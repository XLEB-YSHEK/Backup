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
        /// Creates an archive and returns the path to the saved archive
        /// </summary>
        /// <param name="folders">Array with folders to be included in the archive</param>
        /// <param name="password">Password for the archive</param>
        /// <returns></returns>
        public static string CreateArchive(string[] folders, string[] files, string password)
        {
            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create("ArchiveBackup.zip")))
            {
                zipStream.SetLevel(9);

                zipStream.Password = password;

                for (int floderCounter = 0; floderCounter <= folders.Length - 1; ++floderCounter)
                {
                    AddFolder(folders[floderCounter], folders[floderCounter], zipStream);
                }

                for (int fileCounter = 0; fileCounter <= files.Length - 1; ++fileCounter)
                {
                    AddFile(files[fileCounter], zipStream);
                }

                zipStream.Finish();
                zipStream.Close();
            }          
            return "ArchiveBackup.zip";
        }

        /// <summary>
        /// Adds folders with their contents to the archive
        /// </summary>
        /// <param name="rootFolder"></param>
        /// <param name="currentFolder"></param>
        /// <param name="zipStream"></param>
        private static void AddFolder(string rootFolder, string currentFolder, ZipOutputStream zipStream)
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
                AddFolder(rootFolder, folder, zipStream);
            }
        }

        /// <summary>
        /// Adds files to the archive
        /// </summary>
        /// <param name="filePatch">File path</param>
        /// <param name="zipStream">ZipOutputStream</param>
        private static void AddFile(string filePatch, ZipOutputStream zipStream)
        {
            FileInfo fileInfo = new FileInfo(filePatch);
            DirectoryInfo directory = new DirectoryInfo(fileInfo.Directory.FullName);

            using (FileStream fileStream = File.OpenRead(filePatch))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);

                string entryName = directory.Name + "//" + filePatch.Replace(directory.FullName, "").TrimStart('\\');
                ZipEntry entry = new ZipEntry(entryName);
                entry.DateTime = fileInfo.LastWriteTime;
                entry.Size = fileStream.Length;

                zipStream.PutNextEntry(entry);
                zipStream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// Sends an archive to the Discord channel through a bot
        /// </summary>
        /// <param name="archivePatch">Path to the archive</param>
        /// <param name="botToken">Discord bot token</param>
        /// <param name="channelId">Channel ID</param>
        /// <returns></returns>
        public static async Task SendBackup(string archivePatch, string botToken, string channelId)
        {
            HttpClient HttpClient = new HttpClient();
            byte[] FileContent = File.ReadAllBytes(archivePatch);

            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + botToken);

            MultipartFormDataContent Form = new MultipartFormDataContent
            {
                { new ByteArrayContent(FileContent), "Backup" + DateTime.Now.ToString(), "Backup.zip" }
            };

            await HttpClient.PostAsync($"https://discord.com/api/v9/channels/{channelId}/messages", Form);
        }

        /// <summary>
        /// Checks if it is time to create a backup
        /// </summary>
        /// <returns></returns>
        public static bool TimeToBackup()
        {
            if (File.Exists("NextBackup.txt"))
            {
                DateTime date = DateTime.Parse(File.ReadAllText("NextBackup.txt"));

                if (DateTime.Now > date)
                {
                    File.WriteAllText("NextBackup.txt", DateTime.Now.AddDays(Plugin.Singleton.Config.DayNextBackup).ToShortDateString());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                File.WriteAllText("NextBackup.txt", DateTime.Now.AddDays(Plugin.Singleton.Config.DayNextBackup).ToShortDateString());
                return true;
            }
        }
    }
}
