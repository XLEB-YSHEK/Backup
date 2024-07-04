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

        [Description("Файлы, которые будут включены в бекап. Общий вес максимум 25 мб")]
        public string[] LogFiles { get; set; } = { "/root/.config/Exiled/config.txt", "/root/.config/SCP Secret Lab/key.txt" };

        [Description("Пароль к архиву")]
        public string ArchivePassword { get; set; } = "12345";

        [Description("Токен discord - бота")]
        public string BotToken { get; set; } = "98jfi3j1i2f033";

        [Description("Путь к файлу с ключом шифрования")]
        public string KeyPatch { get; set; } = "/root/key.txt";

        [Description("ID канала")]
        public string ChannelID { get; set; } = "7823749824932";

        [Description("Через сколько дней делать бекап?")]
        public int DayNextBackup { get; set; } = 1;

        [Description("Пофиг")]
        public bool Debug { get; set; } = false;
    }
}
