using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Photo_Gallery_Web_API.Services.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Photo_Gallery_Web_API.Dtos.AuthDtos;
using Photo_Gallery_Web_API.Middleware;

public class AuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<IdentityResult> SignupAsync(SignUp signupDTO)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == signupDTO.Username);
        if (existingUser != null)
        {
            var errors = new List<ValidationError>
            {
                new ValidationError
                {
                    Property = "Username",
                    Message = "Username already exists"
                }
            };
            return IdentityResult.Failed(errors.Select(error => new IdentityError
            {
                Code = error.Property,
                Description = error.Message
            }).ToArray());
        }


        var user = new User
        {
            Username = signupDTO.Username,
            Password = HashPassword(signupDTO.Password),
            RoleType = signupDTO.RoleType
            
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return IdentityResult.Success;
    }

    public async Task<Token> SigninAsync(SignIn signinDTO)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == signinDTO.Username);

        if (user != null && VerifyPassword(signinDTO.Password, user.Password))
        {

            var result = new Token
            {
                token = GenerateJwtToken(user)
            };

            return result;
        }

        return null; // Signin failed
    }

    private string HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    private bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:DurationInMinutes")),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}
