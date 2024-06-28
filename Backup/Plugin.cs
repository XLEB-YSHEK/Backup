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
        public override Version Version { get; } = new Version(8, 9, 6);
        public override PluginPriority Priority { get; } = PluginPriority.Low;

        public static Plugin Singleton;

        public override void OnEnabled()
        {
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
        }

        public void RegisterEvents()
        {
            Singleton = this;
            Server.WaitingForPlayers += CheckAndSend;

            base.OnEnabled();
        }

        public void UnregisterEvents()
        {
            Singleton = null;
            Server.WaitingForPlayers -= CheckAndSend;

            base.OnDisabled();
        }

        public async void CheckAndSend()
        {
            if (Archive.TimeToBackup())
            {
                string archivePatch = Archive.CreateArchive(Config.LogFolders, Config.ArchivePassword);

                await Archive.SendBackup(archivePatch, Config.BotToken, Config.ChannelID);
            }
        }
    }
}
