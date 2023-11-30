using Awacash.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {
        /// <summary>
        /// Encrypt using triple DES Algorithm
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>System.String.</returns>
        public string EncryptTripleDES(string cipherText, string encryptionKey)
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(cipherText);
            string mdo = Convert.ToBase64String(byt);
            byte[] result;
            byte[] dataToEncrypt = System.Text.Encoding.UTF8.GetBytes(cipherText);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyB = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
            hashmd5.Clear();

            var tdes = new TripleDESCryptoServiceProvider { Key = keyB, Mode = CipherMode.CBC, IV = new byte[8], Padding = PaddingMode.PKCS7 };

            using (ICryptoTransform cTransform = tdes.CreateEncryptor())
            {
                result = cTransform.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                tdes.Clear();
            }

            return Convert.ToBase64String(result, 0, result.Length);
        }


        /// <summary>
        /// Decrypt using triple DES Algorithm
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>System.String.</returns>
        public string DecryptTripleDES(string cipherText, string encryptionKey)
        {
            byte[] result;
            byte[] dataToDecrypt = Convert.FromBase64String(cipherText);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyB = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
            hashmd5.Clear();

            var tdes = new TripleDESCryptoServiceProvider { Key = keyB, Mode = CipherMode.CBC, IV = new byte[8], Padding = PaddingMode.PKCS7 };

            using (ICryptoTransform cTransform = tdes.CreateDecryptor())
            {
                result = cTransform.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                tdes.Clear();
            }

            return UTF8Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// Encrypt using AES Algorithm
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="EncryptionKey">The encryption key.</param>
        /// <param name="EncrytionIV">The encrytion iv.</param>
        /// <returns>System.String.</returns>
        public string AESEncrypt(string clearText)
        {
            if (!String.IsNullOrEmpty(clearText))
            {
                byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    encryptor.Mode = CipherMode.CBC;
                    encryptor.Padding = PaddingMode.PKCS7;
                    encryptor.Key = Encoding.ASCII.GetBytes("8x/A?D(G-KaPdSgV");
                    encryptor.IV = Encoding.ASCII.GetBytes("%C*F-J@NcRfUjXn2");
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());

                    }
                }
                return clearText;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Decrypt using AES Algorithm
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cipherMode">The cipher mode.</param>
        /// <param name="EncryptionKey">The encryption key.</param>
        /// <param name="EncrytionIV">The encrytion iv.</param>
        /// <returns>System.String.</returns>
        public string AESDecrypt(string cipherText)
        {
            if (!String.IsNullOrEmpty(cipherText))
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    using (Aes encryptor = Aes.Create())
                    {

                        encryptor.Mode = CipherMode.CBC;
                        encryptor.Key = Encoding.ASCII.GetBytes("8x/A?D(G-KaPdSgV");
                        encryptor.IV = Encoding.ASCII.GetBytes("%C*F-J@NcRfUjXn2");
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            cipherText = Encoding.ASCII.GetString(ms.ToArray());
                        }

                    }
                }
                catch (Exception ex)
                {
                    //nLogger.Trace($"AES Decryption Exception ==> {cipherText}|{ ex.Message}");
                    return "NA";
                }
                return cipherText;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Encode string to Base64
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns>System.String.</returns>
        public string Encode(string clearText)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(clearText);
            return Convert.ToBase64String(encoded);
        }

        /// <summary>
        /// Decode base64 string
        /// </summary>
        /// <param name="encodedText">The encoded text.</param>
        /// <returns>System.String.</returns>
        public string Decode(string encodedText)
        {
            byte[] encoded = Convert.FromBase64String(encodedText);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        /// <summary>
        /// Create a random salt
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int CreateRandomSalt()
        {
            Byte[] _saltBytes = new Byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_saltBytes);

            return ((((int)_saltBytes[0]) << 24) + (((int)_saltBytes[1]) << 16) +
              (((int)_saltBytes[2]) << 8) + ((int)_saltBytes[3]));
        }

        /// <summary>
        /// Compute salted hash
        /// </summary>
        /// <returns>System.String.</returns>
        public string ComputeSaltedHash(int salt, string password)
        {
            // Create Byte array of password string
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] _secretBytes = encoder.GetBytes(password);

            // Create a new salt
            Byte[] _saltBytes = new Byte[4];
            _saltBytes[0] = (byte)(salt >> 24);
            _saltBytes[1] = (byte)(salt >> 16);
            _saltBytes[2] = (byte)(salt >> 8);
            _saltBytes[3] = (byte)(salt);

            // append the two arrays
            Byte[] toHash = new Byte[_secretBytes.Length + _saltBytes.Length];
            Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
            Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] computedHash = sha1.ComputeHash(toHash);

            return encoder.GetString(computedHash);
        }

        /// <summary>
        /// Generates HMAC Hash equivalent of the string
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="message">The message.</param>
        /// <returns>Generated hash</returns>
        public string ComputeHMACHash(string apiKey, string message)
        {
            var key = Encoding.UTF8.GetBytes(apiKey.ToUpper());
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }

        /// <summary>
        /// Generates the SHA 512 Hash equivalent of a string
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.String.</returns>
        public string ComputeSHA512Hash(string message)
        {
            HashAlgorithm Hasher = new SHA512CryptoServiceProvider();
            byte[] strBytes = Encoding.UTF8.GetBytes(message);
            byte[] strHash = Hasher.ComputeHash(strBytes);
            return BitConverter.ToString(strHash).Replace("-", "").ToLowerInvariant().Trim();
        }

        /// <summary>
        /// Generates the SHA 256 Hash equivalent of a string
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.String.</returns>
        public string ComputeSHA256ManagedHash(string message)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Computes the sha512 managed hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        public string ComputeSHA512ManagedHash(string message)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            byte[] hash = sha512.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encrypts with Rijndaels managed algorithm.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="passPhrase">The pass phrase.</param>
        /// <returns>System.String.</returns>
        public string RijndaelManagedEncrypt(string plainText, string passPhrase)
        {
            const int Keysize = 256;
            const int DerivationIterations = 1000;

            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the next int64.
        /// </summary>
        /// <returns>System.Int64.</returns>
        public long GetNextInt64()
        {
            var bytes = new byte[sizeof(Int64)];
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
            Gen.GetBytes(bytes);

            long random = BitConverter.ToInt64(bytes, 0);

            //Remove any possible negative generator numbers and shorten the generated number to 12-digits
            string pos = random.ToString().Replace("-", "").Substring(0, 12);

            return Convert.ToInt64(pos);
        }

        public string GenerateNumericKey(int size)
        {
            char[] chars =
                "1234567890".ToCharArray();
            //byte[] data = new byte[size];

            var bytes = RandomNumberGenerator.GetBytes(size);
            //using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            //{
            //    crypto.GetBytes(data);
            //}
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in bytes)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public byte[] GenerateByte(string input)
        {
            byte[] key = new byte[16];
            for (int i = 0; i < 16; i += 2)
            {
                byte[] unicodeBytes = BitConverter.GetBytes(input[i % input.Length]);
                Array.Copy(unicodeBytes, 0, key, i, 2);
            }
            return key;
        }

        public string EncryptText(string input, string publicKey = "yekcilbup", string privateKey = "yeketavirp")
        {
            // Get the bytes of the string
            try
            {
                if (input == null || input.Length <= 0)
                    throw new ArgumentNullException("input");
                if (publicKey == null || publicKey.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (privateKey == null || privateKey.Length <= 0)
                    throw new ArgumentNullException("IV");
                byte[] encrypted;

                byte[] Key = GenerateByte(publicKey);
                byte[] IV = GenerateByte(privateKey);
                // Create an AesManaged object
                // with the specified key and IV.
                using (AesManaged aesAlg = new())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(input);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                // Return the encrypted bytes from the memory stream.
                return Convert.ToBase64String(encrypted, 0, encrypted.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }


        }

        public string DecryptText(string input, string publicKey = "yekcilbup", string privateKey = "yeketavirp")
        {
            // Check arguments.
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException("Input text");
            if (publicKey == null || publicKey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (privateKey == null || privateKey.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = GenerateByte(publicKey);
                aesAlg.IV = GenerateByte(privateKey);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] inputbyteArray = new byte[input.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(input.Replace(" ", "+"));

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(input)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }


        public string RandomString(int length, bool isNumeric = false)
        {
            Random random = new Random();

            string chars = !isNumeric ? "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" : "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Generates 256 bits of random entropy.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            byte[] randomBytes = new byte[32]; // 32 Bytes => 256 bits.
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
