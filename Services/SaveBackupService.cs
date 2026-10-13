
using System;
using System.IO;

namespace Imperialism2SaveEditor.Services
{
    public class SaveBackupService
    {
        public string CreateBackup(string saveFilePath)
        {
            if (!File.Exists(saveFilePath))
                throw new FileNotFoundException(
                    "The original savegame could not be found.",
                    saveFilePath);

            string directory = Path.GetDirectoryName(saveFilePath)!;
            string fileName = Path.GetFileNameWithoutExtension(saveFilePath);
            string extension = Path.GetExtension(saveFilePath);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fffffff");

            string backupPath = Path.Combine(
                directory,
                $"{fileName}_{timestamp}.backup{extension}");

            File.Copy(saveFilePath, backupPath, overwrite: false);

            return backupPath;
        }
    }
}
