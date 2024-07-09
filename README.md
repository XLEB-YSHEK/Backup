# Backup
This is a small plugin for backing up server files and sending them to the discord channel as a zip archive

Do not forget to set the password to the archive in the configs!

What a backup looks like

![Backup](https://cdn.discordapp.com/attachments/901753786895310888/1260178997938098196/image.png?ex=668e60a6&is=668d0f26&hm=2dadb6823c2af629a6396c7455e29239269c15f165fde0476f3f24acd0c1b783&)

# Installation
Place the `Backup.dll` file in `EXILED/Plugins`

Place `ICSharpCode.SharpZipLib.dll` file in `EXILED/Plugins/dependencies`

If you are not going to encrypt the archive for additional protection, you do not need `BackupConsole.exe`

You need create a new discord bot!
# Configuration
Remember, there are separate settings for saving entire folders and files, if somehow a file or directory is missing, an error may occur and the backup will not be sent. If you only need to save files or folders, then put `[]` so that any of these categories are not saved. Example: `log_files: []`

Note that the plugin will be inactive the first time you run it so you can configure it. The first time it is turned on it should send a backup to the channel, if it does not, check the configuration and delete the `NextBackup.txt` file in the server folder (where LocalAdmin is located)

# Encryption
To use encryption and decryption you must generate an encryption key, for this you must download `BackupConsole.exe`.
1. Select key generation
2. Choose the complexity of the key (in fact, I think no one will try to bruteforce the archive password).
3. Select the folder where the file with the encryption key will be created.
![Key generation](https://cdn.discordapp.com/attachments/901753786895310888/1260174583881928774/image.png?ex=668e5c89&is=668d0b09&hm=37baf9d1166f1b52cdf91e7317068ace3ff4b2238551694f47d11a6f0a1a01df&)

Save this key and put it on the server. In the configs, specify the full path to the key.

# Decryption
Use `BackupConsole.exe`
1. Select the decryption of the file
2. Specify the path to the encryption key (Full path, not folder)
3. Specify the path to the encrypted file

![Decryption](https://cdn.discordapp.com/attachments/901753786895310888/1260177746030624778/image.png?ex=668e5f7b&is=668d0dfb&hm=3c3b0ab1e73598c498668bc04cbd8a397ec02e0d7bcf31b73d4c77012ac658bd&)

Note that the original file will be deleted and replaced by the decrypted one!