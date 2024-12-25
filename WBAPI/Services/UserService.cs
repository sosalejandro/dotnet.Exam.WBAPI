using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WBAPI.Configuration;
using WBAPI.Models;

namespace WBAPI.Services;

public interface IUserService
{
    void Register(User user);
    (User?, TokenDto?) Login(string username, string password);
}

public class UserService : IUserService
{
    private readonly List<User> _users = new List<User>();
    private readonly JwtSettings _jwtSettings;

    public UserService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public void Register(User user)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        user.Id = _users.Count + 1;
        _users.Add(user);
    }

    public (User?, TokenDto?) Login(string username, string password)
    {
        var existingUser = _users.FirstOrDefault(u => u.Username == username);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(password, existingUser.PasswordHash))
        {
            return (null, null);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, existingUser.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (existingUser, new(tokenString));
    }
}