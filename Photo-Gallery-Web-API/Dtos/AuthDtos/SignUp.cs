namespace Photo_Gallery_Web_API.Dtos.AuthDtos;

public class SignUp
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string RoleType { get; set; } = "user";
}
