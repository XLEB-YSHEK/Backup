using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Server = Exiled.Events.Handlers.Server;

namespace Backup
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix { get; } = "Backup";
        public override string Name { get; } = "Backup";
        public override string Author { get; } = "XLEB_YSHEK";
        public override Version Version { get; } = new Version(3, 2, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Low;

        public static Plugin Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            Server.WaitingForPlayers += OnWaitingForPlayers;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= OnWaitingForPlayers;
            Singleton = null;

            base.OnDisabled();
        }

        public async void OnWaitingForPlayers()
        {
            if (Archive.TimeToBackup())
            {
                string archivePatch = Archive.CreateArchive(Config.LogFolders, Config.LogFiles, Config.ArchivePassword);

                if (Config.UseArchiveEncryption)
                {
                    byte[] key = Encrypt.GetKeyFromFile(Config.KeyPatch);
                    archivePatch = Encrypt.EncryptFile(archivePatch, key);
                }

                await Archive.SendBackup(archivePatch, Config.DiscordWebhookUrl);
            }
        }
    }
}
