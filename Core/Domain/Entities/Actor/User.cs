#nullable disable

using Domain.Base;
using Domain.Entities.Guard;
using Domain.Enums;
using Domain.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Domain.Entities.Actor;

[Table("Users", Schema = "Actor")]
public class User : Person, IUpdatable, IDeletable
{
    public User()
    {
        Status = UserStatus.Normal;
    }

    public UserStatus Status { get; private set; }

    [MinLength(2), MaxLength(100), EmailAddress]
    public string? Email { get; set; }
    public string Password { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpire { get; private set; }
    public int? VerifyCode { get; private set; }
    public DateTime? VerifyCodeExpire { get; private set; }
    public DateTime? VerifyCodeDate { get; private set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<Membership> Memberships { get; set; }

    public void Block()
    {
        Status = UserStatus.Block;
    }

    public void UnBlock()
    {
        Status = UserStatus.Normal;
    }

    public void SetPassword(string password)
    {
        Password = PBKDF2Hasher.Hash(password);
    }

    public bool CheckPassword(string password)
    {
        return PBKDF2Hasher.Check(Password, password);
    }

    public void SetRefreshToken(string token)
    {
        RefreshToken = token;
        RefreshTokenExpire = DateTime.UtcNow.AddDays(7);
    }

    public void GenerateVerificationCode(int offset = 2)
    {
        VerifyCode = RandomNumberGenerator.GetInt32(10000, 99999);
        VerifyCodeExpire = DateTime.UtcNow.AddMinutes(offset);
    }

    public bool Verify(int code)
    {
        if (code != VerifyCode) return false;
        VerifyCodeDate = DateTime.UtcNow;

        return true;
    }
}
