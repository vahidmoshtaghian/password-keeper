using System.Security.Cryptography;

namespace Domain.Utilities;

public class PBKDF2Hasher
{
    public static string Hash(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            PBKDF2Settings.SaltSize,
            PBKDF2Settings.Iterations,
            HashAlgorithmName.SHA256);
        var key = Convert.ToBase64String(algorithm.GetBytes(PBKDF2Settings.KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{PBKDF2Settings.Iterations}.{salt}.{key}";
    }

    public static bool Check(string hash, string password)
    {
        var parts = hash.Split('.', 3);

        if (parts.Length != 3)
        {
            throw new FormatException("Unexpected hash format. " +
              "Should be formatted as `{iterations}.{salt}.{hash}`");
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        using var algorithm = new Rfc2898DeriveBytes(
          password,
          salt,
          iterations,
          HashAlgorithmName.SHA256);
        var keyToCheck = algorithm.GetBytes(PBKDF2Settings.KeySize);

        var verified = keyToCheck.SequenceEqual(key);

        return verified;
    }
}

internal class PBKDF2Settings
{
    public const int SaltSize = 16;
    public const int KeySize = 32;
    public const int Iterations = 10000;
}