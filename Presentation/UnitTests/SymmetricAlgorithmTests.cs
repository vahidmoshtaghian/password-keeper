using AesEncryptor;

namespace UnitTests;

public class SymmetricAlgorithmTests
{
    [Fact]
    public void SymmetricAlgorithm_ShouldEncript()
    {
        // Arrange
        var key = "thisIsAKeyThisIsAKeyThisIsAKey12";
        var expected = "ThisIsMyPassword123456!@#$%^";
        var sut = new AesEncryptorService(key);

        // Act
        var encrypted = sut.Encrypt(expected);
        var actual = sut.Decrypt(encrypted);

        // Assert
        Assert.Equal(expected, actual);
    }
}
