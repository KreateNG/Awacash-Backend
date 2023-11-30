using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Common.Interfaces.Services
{
    public interface ICryptoService
    {
        /// <summary>
        /// Encrypts with triple DES.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns>System.String.</returns>
        string EncryptTripleDES(string cipherText, string encryptionKey);

        /// <summary>
        /// Decrypts with triple DES.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns>System.String.</returns>
        string DecryptTripleDES(string cipherText, string encryptionKey);

        /// <summary>
        /// AES encrypt.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="EncryptionKey">The encryption key.</param>
        /// <param name="EncrytionIV">The encrytion iv.</param>
        /// <returns>System.String.</returns>
        string AESEncrypt(string clearText);

        /// <summary>
        /// AES decrypt.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="EncryptionKey">The encryption key.</param>
        /// <param name="EncrytionIV">The encrytion iv.</param>
        /// <returns>System.String.</returns>
        string AESDecrypt(string cipherText);

        /// <summary>
        /// Encodes the specified clear text.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns>System.String.</returns>
        string Encode(string clearText);

        /// <summary>
        /// Decodes the specified encoded text.
        /// </summary>
        /// <param name="encodedText">The encoded text.</param>
        /// <returns>System.String.</returns>
        string Decode(string encodedText);

        /// <summary>
        /// Creates the random salt.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int CreateRandomSalt();

        /// <summary>
        /// Computes the salted hash.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        string ComputeSaltedHash(int salt, string password);

        /// <summary>
        /// Computes the hmac hash.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        string ComputeHMACHash(string apiKey, string message);

        /// <summary>
        /// Computes the sha512 hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        string ComputeSHA512Hash(string message);

        /// <summary>
        /// Computes the sha256 managed hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        string ComputeSHA256ManagedHash(string message);

        /// <summary>
        /// Computes the sha512 managed hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        string ComputeSHA512ManagedHash(string message);

        /// <summary>
        /// Encrypts with Rijndaels managed algorithm.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="passPhrase">The pass phrase.</param>
        /// <returns>System.String.</returns>
        string RijndaelManagedEncrypt(string plainText, string passPhrase);

        /// <summary>
        /// Gets the next int64.
        /// </summary>
        /// <returns>System.Int64.</returns>
        long GetNextInt64();
        string GenerateNumericKey(int size);

        string DecryptText(string input, string publicKey = "yekcilbup", string privateKey = "yeketavirp");
        string EncryptText(string input, string publicKey = "yekcilbup", string privateKey = "yeketavirp");

        string RandomString(int length, bool isNumeric = false);
    }
}
