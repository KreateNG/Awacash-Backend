using System;
using System.Security.Cryptography;
using System.Text;
using Awacash.Domain.Entities;

namespace Awacash.Domain.Extentions
{
    public static class WalletExtention
    {
        public static bool ValidateCheckSum(this Wallet wallet)
        {
            bool isValid = false;
            try
            {
                var decryptedValue = AESDecrypt(wallet.CheckSum);
                var values = decryptedValue.Split("|");
                if (values.Count() == 5)
                {
                    string walletId = values[0];
                    string firstName = values[1];
                    string lastName = values[2];
                    string phoneNumber = values[3];
                    decimal.TryParse(values[4], out decimal balance);

                    if (wallet.Id == walletId && firstName == wallet.FirstName && lastName == wallet.LastName && phoneNumber == wallet.PhoneNumber && balance == wallet.Balance)
                        isValid = true;
                }
            }
            catch
            { }

            return isValid;
        }


        public static string GetCheckSum(this Wallet wallet)
        {
            string checkSumValue = "";
            try
            {
                string data = string.Concat(wallet.Id, "|", wallet.FirstName, "|", wallet.LastName, "|", wallet.PhoneNumber, "|", wallet.Balance.ToString());
                checkSumValue = AESEncrypt(data);
            }
            catch
            { }

            return checkSumValue;
        }

        private static string AESEncrypt(string clearText)
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

        public static string AESDecrypt(string cipherText)
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

    }
}

