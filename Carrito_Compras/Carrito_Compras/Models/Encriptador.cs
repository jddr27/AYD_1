using System;
using System.IO;
using System.Security.Cryptography;

namespace Carrito_Compras.Models
{
    public class Encriptador
    {

        public string enc { get; set; }
        public string des { get; set; }

        public Encriptador(string original)
        {
            try
            {
                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.Key = new byte[] { 201, 141, 160, 149, 24, 116, 240, 172, 124, 189, 18, 19, 108, 25, 66, 7, 173, 185, 109, 110, 221, 108, 219, 61, 164, 127, 138, 247, 93, 221, 63, 152 };
                    myRijndael.IV = new byte[] { 242, 150, 76, 242, 34, 134, 218, 50, 74, 113, 72, 154, 153, 214, 226, 165 };
                    byte[] arreglo = EncryptStringToBytes(original, myRijndael.Key, myRijndael.IV);
                    enc = "";
                    foreach (Byte b in arreglo)
                    {
                        enc += b + ",";
                    }
                    enc = enc.Substring(0, enc.Length - 1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("***************************** Error: {0}", e.Message);
            }
        }

        public Encriptador(byte[] encrypted)
        {
            try
            {
                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.Key = new byte[] { 201, 141, 160, 149, 24, 116, 240, 172, 124, 189, 18, 19, 108, 25, 66, 7, 173, 185, 109, 110, 221, 108, 219, 61, 164, 127, 138, 247, 93, 221, 63, 152 };
                    myRijndael.IV = new byte[] { 242, 150, 76, 242, 34, 134, 218, 50, 74, 113, 72, 154, 153, 214, 226, 165 };
                    des = DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);
                    Console.WriteLine("SALIDA!!!!!!!!!!!!!!!!!!! " + des);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("****************************** Error: {0}", e.Message);
            }
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
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
    }
}