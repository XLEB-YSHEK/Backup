using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Backup
{
    public class Config : IConfig
    {
        [Description("Plugin enabled?")]
        public bool IsEnabled { get; set; } = false;

        [Description("Directories to be saved")]
        public string[] LogFolders { get; set; } = { "/root/.config/EXILED", "/root/.config/SCP Secret Lab" };

        [Description("Files to be saved")]
        public string[] LogFiles { get; set; } = { "/root/.config/Exiled/config.txt", "/root/.config/SCP Secret Lab/key.txt" };

        [Description("Password to the archive")]
        public string ArchivePassword { get; set; } = "12345";

        [Description("Discord Bot token")]
        public string DiscordBotToken { get; set; } = "98jfi3j1i2f033";

        [Description("Use archive encryption?")]
        public bool UseArchiveEncryption { get; set; } = true;

        [Description("Path to the file with the encryption key")]
        public string KeyPatch { get; set; } = "/root/key.txt";

        [Description("ID of the channel to which the backup will be sent")]
        public string ChannelID { get; set; } = "7823749824932";

        [Description("How many days to do a backup?")]
        public int DayNextBackup { get; set; } = 1;

        [Description("Debug")]
        public bool Debug { get; set; } = false;
    }
}
