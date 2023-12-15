using Domain.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace AesEncryptor;

public class AesEncryptorService : IEncryptor
{
    private readonly string _key;

    public AesEncryptorService(string? key)
    {
        _key = key;
    }

    public string Encrypt(string text)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = new byte[16];

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
        using (StreamWriter streamWriter = new(cryptoStream))
        {
            streamWriter.Write(text);
        }

        var result = memoryStream.ToArray();

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipher)
    {
        var buffer = Convert.FromBase64String(cipher);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = new byte[16];
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream memoryStream = new(buffer);
        using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        return streamReader.ReadToEnd();
    }
}