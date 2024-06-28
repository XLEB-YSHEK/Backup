using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Backup
{
    public class Config : IConfig
    {
        [Description("Включён?")]
        public bool IsEnabled { get; set; } = false;

        [Description("Папки, которые будут включены в бекап. Общий вес максимум 25 мб")]
        public string[] LogFolders { get; set; } = { "/root/.config/Exiled", "/root/.config/SCP Secret Lab" };

        [Description("Пароль к архиву")]
        public string ArchivePassword { get; set; } = "12345";

        [Description("Токен discord - бота")]
        public string BotToken { get; set; } = "98jfi3j1i2f033";

        [Description("ID канала")]
        public string ChannelID { get; set; } = "7823749824932";

        [Description("Через сколько дней делать бекап?")]
        public int DayNextBackup { get; set; } = 1;

        [Description("Пофиг")]
        public bool Debug { get; set; } = false;
    }
}
