using System;
using System.ComponentModel;
using System.IO;
using Exiled.API.Interfaces;

namespace Backup
{
    public class Config : IConfig
    {
        [Description("Indicates plugin enabled or not.")]
        public bool IsEnabled { get; set; } = false;

        [Description("Directories to be saved.")]
        public string[] LogFolders { get; set; } = { Path.Combine(Encrypt.AppData, "EXILED"), Path.Combine(Encrypt.AppData, "SCP Secret Laboratory") };

        [Description("Files to be saved.")]
        public string[] LogFiles { get; set; } = { Path.Combine(Encrypt.AppData, "EXILED", "7777-config.yml"), Path.Combine(Encrypt.AppData, "SCP Secret Laboratory", "verkey.txt") };

        [Description("Password to the archive")]
        public string ArchivePassword { get; set; } = "12345";

        [Description("Discord Webhook URL")]
        public string DiscordWebhookUrl { get; set; } = "Webhook full URL";

        [Description("Use archive encryption?")]
        public bool UseArchiveEncryption { get; set; } = true;

        [Description("Path to the file where encryption key stored.")]
        public string KeyPatch { get; set; } = Path.Combine(Encrypt.AppData, "access-key.txt");

        [Description("How many days to do a backup?")]
        public int DayNextBackup { get; set; } = 1;

        [Description("Debug")]
        public bool Debug { get; set; } = false;
    }
}
