using System.IO;
using System.Security.Cryptography;

namespace Backup
{
    public class Encrypt
    {
        public static byte[] GetKeyFromFile(string filePatch)
        {
            return File.ReadAllBytes(filePatch);
        }

        public static string EncryptFile(string filePatch, byte[] key)
        {
            if (key == null || !ValidateKey(key) || !File.Exists(filePatch))
            {
                return null;
            }

            var tempPatch = Path.GetTempFileName();
            var encryptFileName = "Encrypt" + new FileInfo(filePatch).Name;
            var targetDirectory = new FileInfo(filePatch).Directory.FullName;
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
