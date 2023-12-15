namespace Domain.Contracts;

public interface IEncryptor
{
    string Encrypt(string text);
    string Decrypt(string cipher);
}
