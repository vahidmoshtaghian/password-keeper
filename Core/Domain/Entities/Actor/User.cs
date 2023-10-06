using Domain.Base;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Domain.Entities.Actor;

public class User : Person, IUpdatable, IDeletable
{
    public User()
    {
        Status = UserStatus.Normal;
    }

    public UserStatus Status { get; private set; }

    [MinLength(2), MaxLength(100)]
    public string? Email { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpire { get; private set; }
    public int? VerifyCode { get; private set; }
    public DateTime? VerifyCodeExpire { get; private set; }
    public DateTime? VerifyCodeDate { get; private set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public void Block()
    {
        Status = UserStatus.Block;
    }

    public void UnBlock()
    {
        Status = UserStatus.Normal;
    }

    public void SetRefreshToken(string token)
    {
        RefreshToken = token;
        RefreshTokenExpire = DateTime.Now.AddDays(7);
    }

    public void GenerateVerificationCode(int offset = 2)
    {
        VerifyCode = RandomNumberGenerator.GetInt32(10000, 99999);
        VerifyCodeExpire = DateTime.Now.AddMinutes(offset);
    }

    public bool Verify(int code)
    {
        if (code != VerifyCode) return false;
        VerifyCodeDate = DateTime.Now;

        return true;
    }
}
