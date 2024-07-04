using System.Security.Cryptography;

namespace BackupConsole
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Действие:\n[1] - Генерация ключа\n[2] - Дешифровка файла\n");
            string choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    {
                        Console.WriteLine("Безопасность ключа:\n[1] - Низкая\n[2] - Средняя\n[3] - Высокая\n");
                        string keyChoice = Console.ReadLine();

                        Console.WriteLine("Папка для сохранения ключа\n");
                        string directory = Console.ReadLine();

                        GenerateKeyToFile(directory, keyChoice == "1" ? KeyLenght.Small : keyChoice == "2" ? KeyLenght.Medium : KeyLenght.Long);
                        break;
                    }
                    case "2":
                    {
                        Console.WriteLine("Путь до ключа шифрования:\n");
                        string keyPatch = Console.ReadLine();

                        byte[] key = GetKeyFromFile(keyPatch);

                        Console.WriteLine("Путь до файла для дешифровки\n");
                        string filePatch = Console.ReadLine();

                        DecryptFile(filePatch, key);
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
        /// Генерирует файл с ключом шифрования в указанную директорию
        /// </summary>
        /// <param name="patchDirectory">Путь до каталога, где будет храниться файл с ключом</param>
        /// <param name="keyLenght">Длина генерируемого ключа</param>
        /// <returns>Возвращает путь до созданного файла с ключом</returns>
        public static void GenerateKeyToFile(string patchDirectory, KeyLenght keyLenght)
        {
            using (FileStream FileStream = new FileStream(Path.Combine(patchDirectory, "key.txt"), FileMode.Create))
            {
                AesManaged aesManaged = new AesManaged();
                aesManaged.KeySize = (int)keyLenght;

                aesManaged.GenerateKey();
                FileStream.Write(aesManaged.Key, 0, aesManaged.Key.Length);
            }
            Console.WriteLine("Путь до файла: " + Path.Combine(patchDirectory, "key.txt"));
        }

        /// <summary>
        /// Производит расшифровку файла при помощи ключа шифрования. Удаляет файл, который был зашифрован, на его место ставит расшифрованный
        /// </summary>
        /// <param name="filePatch">Путь до файла</param>
        /// <param name="key">Ключ шифрования</param>
        /// <returns>Возвращает true или false в зависимости от успеха операции</returns>
        public static bool DecryptFile(string filePatch, byte[] key)
        {
            if (key == null || !ValidateKey(key) || !File.Exists(filePatch))
            {
                return false;
            }

            try
            {
                var tempPath = Path.GetTempFileName();

                using (FileStream fileStreamSource = File.OpenRead(filePatch))
                {
                    byte[] initializationVector = new byte[16];
                    fileStreamSource.Read(initializationVector, 0, initializationVector.Length);
                    using (AesManaged aesManaged = new AesManaged() { Key = key, IV = initializationVector })
                    using (CryptoStream cryptoStream = new CryptoStream(fileStreamSource, aesManaged.CreateDecryptor(), CryptoStreamMode.Read, true))
                    using (FileStream fileStreamDestination = File.Create(tempPath))
                    {
                        cryptoStream.CopyTo(fileStreamDestination);
                    }
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
        /// Проверяет ключ шифрования на валидность
        /// </summary>
        /// <param name="key">Ключ шифрования</param>
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
        /// Получает ключ шифрования из указанного файла
        /// </summary>
        /// <param name="filePatch">Файл с ключом шифрования</param>
        public static byte[] GetKeyFromFile(string filePatch)
        {
            return File.ReadAllBytes(filePatch);
        }

        /// <summary>
        /// Длина ключа
        /// </summary>
        public enum KeyLenght : int
        {
            Small = 128,
            Medium = 192,
            Long = 256
        }
    }
}