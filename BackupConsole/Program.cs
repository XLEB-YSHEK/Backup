using System.Security.Cryptography;

namespace BackupConsole
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Action:\n[1] - Encryption key generation\n[2] - File decryption\n");

            string choice = Console.ReadLine();

            Console.Clear();

            switch (choice)
            {
                case "1":
                    {
                        Console.WriteLine("Key security:\n[1] - Low\n[2] - Medium\n[3] - High\n");
                        string keyChoice = Console.ReadLine();

                        Console.WriteLine("Folder for saving the key\n");
                        string directory = Console.ReadLine();

                        GenerateKeyToFile(directory, keyChoice == "1" ? KeyLenght.Small : keyChoice == "2" ? KeyLenght.Medium : KeyLenght.Long);
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("Path to encryption key:\n");
                        string keyPatch = Console.ReadLine();

                        byte[] key = GetKeyFromFile(keyPatch);

                        Console.WriteLine("Full file path\n");
                        string filePatch = Console.ReadLine();

                        if (DecryptFile(filePatch, key))
                        {
                            Console.WriteLine("Succes");
                        }
                        else
                        {
                            Console.WriteLine("Failed");
                        }

                        Console.ReadLine();
                        break;
                    }
                default:
                    {
                        Main();
                        break;
                    }
            }
        }

        /// <summary>
        /// Generates a file with the encryption key to the specified directory
        /// </summary>
        /// <param name="patchDirectory">Path to the directory where the key file will be stored</param>
        /// <param name="keyLenght">Generated key length</param>
        /// <returns>Returns the path to the created key file</returns>
        public static void GenerateKeyToFile(string patchDirectory, KeyLenght keyLenght)
        {
            using (FileStream FileStream = new FileStream(Path.Combine(patchDirectory, "key.txt"), FileMode.Create))
            {
                AesManaged aesManaged = new AesManaged();
                aesManaged.KeySize = (int)keyLenght;

                aesManaged.GenerateKey();
                FileStream.Write(aesManaged.Key, 0, aesManaged.Key.Length);
            }
            Console.WriteLine("File path: " + Path.Combine(patchDirectory, "key.txt"));
            Console.ReadLine();
        }

        /// <summary>
        /// Decrypts the file using the encryption key. Deletes the file that was encrypted and replaces it with the decrypted one
        /// </summary>
        /// <param name="filePatch">File path</param>
        /// <param name="key">Encryption key</param>
        /// <returns>Returns true or false depending on the success of the operation</returns>
        public static bool DecryptFile(string filePatch, byte[] key)
        {
            if (key == null || !ValidateKey(key) || !File.Exists(filePatch))
            {
                return false;
            }

            try
            {
                string tempPath = Path.GetTempFileName();

                using (FileStream fileStreamSource = File.OpenRead(filePatch))
                {
                    byte[] initializationVector = new byte[16];

                    fileStreamSource.Read(initializationVector, 0, initializationVector.Length);

                    using AesManaged aesManaged = new AesManaged() { Key = key, IV = initializationVector };
                    using CryptoStream cryptoStream = new CryptoStream(fileStreamSource, aesManaged.CreateDecryptor(), CryptoStreamMode.Read, true);
                    using FileStream fileStreamDestination = File.Create(tempPath);
                    cryptoStream.CopyTo(fileStreamDestination);
                }

                File.Delete(filePatch);
                File.Move(tempPath, filePatch);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the encryption key for correctness
        /// </summary>
        /// <param name="key">Encryption key</param>
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

        /// <summary>
        /// Gets the encryption key from the specified file
        /// </summary>
        /// <param name="filePatch">Encryption key file</param>
        public static byte[] GetKeyFromFile(string filePatch)
        {
            return File.ReadAllBytes(filePatch);
        }

        /// <summary>
        /// Key length
        /// </summary>
        public enum KeyLenght : int
        {
            Small = 128,
            Medium = 192,
            Long = 256
        }
    }
}