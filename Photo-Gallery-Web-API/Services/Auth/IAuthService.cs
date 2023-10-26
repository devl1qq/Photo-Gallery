using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Photo_Gallery_Web_API.Dtos.AuthDtos;

namespace Photo_Gallery_Web_API.Services.Auth;

public interface IAuthService
{
    Task<IdentityResult> SignupAsync(SignUp signupDTO);
    Task<Token> SigninAsync(SignIn signinDTO);
}
