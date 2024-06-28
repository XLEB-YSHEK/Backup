using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace Backup
{
    public class Archive
    {
        /// <summary>
        /// Создёт архив и возвращает путь до сохранённого архива
        /// </summary>
        /// <param name="folders">Массив с папками, которые будут включены в архив</param>
        /// <param name="password">Пароль для архива</param>
        /// <returns></returns>
        public static string CreateArchive(string[] folders, string password)
        {
            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create("ArchiveBackup.zip")))
            {
                zipStream.SetLevel(9);

                zipStream.Password = password;

                for (int floderCounter = 0; floderCounter <= folders.Length - 1; ++floderCounter)
                {
                    CompressFolder(folders[floderCounter], folders[floderCounter], zipStream);
                }

                zipStream.Finish();
                zipStream.Close();
            }          
            return "ArchiveBackup.zip";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolder"></param>
        /// <param name="currentFolder"></param>
        /// <param name="zipStream"></param>
        private static void CompressFolder(string rootFolder, string currentFolder, ZipOutputStream zipStream)
        {
            string[] files = Directory.GetFiles(currentFolder);

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                DirectoryInfo directory = new DirectoryInfo(rootFolder);

                using (FileStream fileStream = File.OpenRead(file))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);

                    string entryName = directory.Name + "//" + file.Replace(rootFolder, "").TrimStart('\\');
                    ZipEntry entry = new ZipEntry(entryName);
                    entry.DateTime = fileInfo.LastWriteTime;
                    entry.Size = fileStream.Length;

                    zipStream.PutNextEntry(entry);
                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }

            string[] folders = Directory.GetDirectories(currentFolder);

            foreach (string folder in folders)
            {
                CompressFolder(rootFolder, folder, zipStream);
            }
        }

        /// <summary>
        /// Отправляет архив в дискорд канал через бота
        /// </summary>
        /// <param name="archivePatch">Путь до архива</param>
        /// <param name="botToken">Токен от бота</param>
        /// <param name="channelId">ID канала</param>
        /// <returns></returns>
        public static async Task SendBackup(string archivePatch, string botToken, string channelId)
        {
            HttpClient HttpClient = new HttpClient();
            byte[] FileContent = System.IO.File.ReadAllBytes(archivePatch);

            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + botToken);

            MultipartFormDataContent Form = new MultipartFormDataContent
            {
                { new ByteArrayContent(FileContent), "Backup" + DateTime.Now.ToString(), "Backup.zip" }
            };

            await HttpClient.PostAsync($"https://discord.com/api/v9/channels/{channelId}/messages", Form);
        }

        /// <summary>
        /// Проверяет, наступило - ли время для создания бекапа
        /// </summary>
        /// <returns></returns>
        public static bool TimeToBackup()
        {
            if (File.Exists("LastBackup.txt"))
            {
                DateTime date = DateTime.Parse(File.ReadAllText("LastBackup.txt"));

                if (DateTime.Now > date)
                {
                    File.WriteAllText("LastBackup.txt", DateTime.Now.AddDays(Plugin.Singleton.Config.DayNextBackup).ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                File.WriteAllText("LastBackup.txt", DateTime.Now.AddDays(Plugin.Singleton.Config.DayNextBackup).ToString());
                return true;
            }
        }
    }
}
