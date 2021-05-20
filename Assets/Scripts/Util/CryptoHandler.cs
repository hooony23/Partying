using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Cryptography;


namespace Util
{
    public class CryptoHandler
    {
        private static string cryptoKey = "pYWYZyEAQy5Z3BSN";
        //AES_128 암호화
        public static String AESEncrypt(String Input)
        {

            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(Input);
            byte[] Salt = Encoding.ASCII.GetBytes(cryptoKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(cryptoKey, Salt);
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);
            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string EncryptedData = Convert.ToBase64String(CipherBytes);

            return EncryptedData;
        }

        //AE_S128 복호화
        public static String AESDecrypt(String Input)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] EncryptedData = Convert.FromBase64String(Input);
            byte[] Salt = Encoding.ASCII.GetBytes(cryptoKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(cryptoKey, Salt);
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(EncryptedData);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

            byte[] PlainText = new byte[EncryptedData.Length];

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

            return DecryptedData;
        }
    }
}
