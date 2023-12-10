using Domain.Base;
using Domain.Contracts;
using Domain.Entities.Actor;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtIdentity;

public class AuthentiaionService : IIdentityService
{
    private readonly CurrentUser _currentUser;

    public AuthentiaionService(CurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string CreateToken()
    {
        List<Claim> claims = new()
        {
            new Claim("id", _currentUser.Id.ToString()),
            new Claim("firstName", _currentUser.FirstName),
            new Claim("lastName", _currentUser.LastName),
            new Claim("mobile", _currentUser.Mobile),
            new Claim("email", _currentUser.Email),
            new Claim("status", _currentUser.Status.ToString()),
            new Claim("verify", _currentUser.IsVerified.ToString()),
        };

        return CreateToken(claims);
    }

    public string CreateToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim("id", user.Id.ToString()),
            new Claim("fullName", user.FullName),
            new Claim("status", user.Status.ToString()),
            new Claim("verify", (user.VerifyCodeDate != null).ToString()),
        };

        return CreateToken(claims);
    }

    private string CreateToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.SecretKey));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return tokenString;
    }

    public string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
