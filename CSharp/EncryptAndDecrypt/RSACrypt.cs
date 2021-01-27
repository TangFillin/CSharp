using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EncryptAndDecrypt
{
    /// <summary>
    /// RSA加密与解密,非对称加密
    /// </summary>
    public static class RSACrypt
    {
        /// <summary>
        /// 试用测试
        /// </summary>
        public static void Test()
        {
            try
            {
                UnicodeEncoding byteConverter = new UnicodeEncoding();

                byte[] dataToEncrypt = byteConverter.GetBytes("Happy Birthday!");
                byte[] encryptedData;
                byte[] decryptedData;

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    //数据加密 获取public key信息
                    //(using RSACryptoServiceProvider.ExportParameters(false)
                    encryptedData = EncryptRSA(dataToEncrypt, rsa.ExportParameters(false), false);
                    Console.WriteLine("Encrypt String:" + byteConverter.GetString(encryptedData));
                    //数据解密，获取private key信息
                    //(using RSACryptoServiceProvider.ExportParameters(true))
                    decryptedData = DecryptRSA(encryptedData, rsa.ExportParameters(true), false);
                    Console.WriteLine("Decrypted String: " + byteConverter.GetString(decryptedData));
                }

                Console.ReadKey();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Encryption failed!");
            }
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="dataToEncrypt">要加密的byte数组</param>
        /// <param name="rsaKeyInfo"></param>
        /// <param name="doOAEPadding"></param>
        /// <returns></returns>
        public static byte[] EncryptRSA(byte[] dataToEncrypt, RSAParameters rsaKeyInfo, bool doOAEPadding)
        {
            try
            {
                byte[] encryptedData;

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    //导入RSA key信息，这里导入公钥信息
                    rsa.ImportParameters(rsaKeyInfo);

                    //加密传入的byte数组，并指定OAEP padding
                    //OAEP padding只可用在微软Window xp及以后的系统中
                    encryptedData = rsa.Encrypt(dataToEncrypt, doOAEPadding);
                }

                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="dataToDecrypt"></param>
        /// <param name="rsaKeyInfo"></param>
        /// <param name="doOAEPPadding"></param>
        /// <returns></returns>
        public static byte[] DecryptRSA(byte[] dataToDecrypt, RSAParameters rsaKeyInfo, bool doOAEPPadding)
        {
            try
            {
                byte[] decryptedData;

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaKeyInfo);

                    decryptedData = rsa.Decrypt(dataToDecrypt, doOAEPPadding);
                }

                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
