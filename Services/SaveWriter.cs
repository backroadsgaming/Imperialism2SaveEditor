
using System;
using System.IO;
using Imperialism2SaveEditor.Core;

namespace Imperialism2SaveEditor.Services
{
    public class SaveWriter
    {
        private readonly SaveBackupService _backupService = new();

        public string SaveChanges(ImperialismSaveFile save)
        {
            if (!save.IsLoaded)
                throw new InvalidOperationException(
                    "No savegame has been loaded.");

            if (string.IsNullOrWhiteSpace(save.FilePath))
                throw new InvalidOperationException(
                    "The savegame path is invalid.");

            if (!File.Exists(save.FilePath))
                throw new FileNotFoundException(
                    "The original savegame no longer exists.",
                    save.FilePath);


            if (save.Data.Length != new FileInfo(save.FilePath).Length)
                throw new InvalidOperationException(
                    "Savegame file size has changed. Reload it before saving.");

            byte[] currentDiskData = File.ReadAllBytes(save.FilePath);

            if (currentDiskData.Length != save.OriginalData.Length ||
                !currentDiskData.AsSpan().SequenceEqual(save.OriginalData))
            {
                throw new InvalidOperationException(
                    "The savegame has been modified since it was loaded. " +
                    "Reload the savegame before saving changes.");
            }

            string directory = Path.GetDirectoryName(save.FilePath)!;

            string temporaryPath = Path.Combine(
                directory,
                $"{Guid.NewGuid():N}.tmp");

            string recoveryPath = Path.Combine(
                directory,
                $"{Path.GetFileName(save.FilePath)}.{Guid.NewGuid():N}.recovery");

            try
            {
                // Create an independent backup before modifying the file.
                string backupPath =
                    _backupService.CreateBackup(save.FilePath);

                // Write modified data to a separate temporary file.
                File.WriteAllBytes(temporaryPath, save.Data);

                // Confirm the temporary file has the expected size.
                if (new FileInfo(temporaryPath).Length != save.Data.Length)
                    throw new IOException(
                        "Temporary savegame verification failed.");

                // Atomically replace the original on the same volume.
                // Windows also preserves the previous file as a recovery copy.
                File.Replace(
                    temporaryPath,
                    save.FilePath,
                    recoveryPath);

                return backupPath;
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }
    }
}
