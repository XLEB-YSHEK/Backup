using System;
using System.IO;
using System.Security.Cryptography;

namespace Backup
{
    public class Encrypt
    {
        /// <summary>
        /// Gets the ApplicationData path directory.
        /// </summary>
        public static string AppData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// Gets the encryption key from a file as an array of bytes
        /// </summary>
        /// <param name="filePatch">Path to the file with the encryption key</param>
        /// <returns></returns>
        public static byte[] GetKeyFromFile(string filePatch)
        {
            return File.ReadAllBytes(filePatch);
        }

        /// <summary>
        /// Encrypts the file
        /// </summary>
        /// <param name="filePatch">Path to the file to be encrypted</param>
        /// <param name="key">A key representing an array of bytes</param>
        /// <returns></returns>
        public static string EncryptFile(string filePatch, byte[] key)
        {
            if (key == null || !ValidateKey(key) || !File.Exists(filePatch))
            {
                return null;
            }

            string tempPatch = Path.GetTempFileName();
            string encryptFileName = "Encrypt" + new FileInfo(filePatch).Name;
            string targetDirectory = new FileInfo(filePatch).Directory.FullName;
            string destinationFile = Path.Combine(targetDirectory, encryptFileName);

            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }

            using (FileStream FileStreamOne = File.OpenRead(filePatch))
            {
                using (AesManaged aesManaged = new AesManaged() { Key = key })
                {
                    using (FileStream fileStreamTwo = File.Create(tempPatch))
                    {
                        fileStreamTwo.Write(aesManaged.IV, 0, aesManaged.IV.Length);

                        using (CryptoStream CryptoStream = new CryptoStream(fileStreamTwo, aesManaged.CreateEncryptor(), CryptoStreamMode.Write, true))
                        {
                            FileStreamOne.CopyTo(CryptoStream);
                        }
                    }
                }
            }
            File.Move(tempPatch, destinationFile);
            return destinationFile;
        }

        /// <summary>
        /// Checks the key for correctness
        /// </summary>
        /// <param name="key">A key representing an array of bytes</param>
        /// <returns></returns>
        private static bool ValidateKey(byte[] key)
        {
            if (key.Length == 16 || key.Length == 24 || key.Length == 32)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
