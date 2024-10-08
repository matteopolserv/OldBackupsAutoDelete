using OldBackupsAutoDelete.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldBackupsAutoDelete
{
    internal class SearchAndDeleteFiles(Logger logger)
    {        
        public async Task DeleteHostingerBackups()
        {
            try
            {
                string directoriesBackupPath = Path.Combine("/volume1", "hostingerBackups");
                DirectoryInfo directoriesBackup = new DirectoryInfo(directoriesBackupPath);
                try
                {
                    var filesToDelete = directoriesBackup.GetFiles("*.*", SearchOption.AllDirectories)
                                .Where(f => f.CreationTime.AddDays(60) < DateTime.Now ||
                            (f.CreationTime.AddDays(10) < DateTime.Now && f.CreationTime.DayOfWeek is not DayOfWeek.Wednesday)).ToList();
                    Parallel.ForEach(filesToDelete, f =>
                    {
                        File.Delete(f.FullName);
                    });
                }
                catch (Exception ex)
                {
                    await logger.SaveInformationToLog($"DeleteHostingerBackups - Usuwanie plików - {ex.Message}");
                }
                try
                {
                    List<DirectoryInfo> emptyDirectories = directoriesBackup.GetDirectories("*.*", SearchOption.AllDirectories)
                    .Where(d => d.GetFiles("*.*", SearchOption.TopDirectoryOnly).Length == 0 && d.GetDirectories("*.*", SearchOption.TopDirectoryOnly).Length == 0)
                    .ToList();
                    Parallel.ForEach(emptyDirectories, emptyDirectory =>
                    {
                        emptyDirectory.Delete(false);
                    });
                }
                catch(Exception ex)
                {
                    await logger.SaveInformationToLog($"DeleteHostingerBackups - Usuwanie katalogów - {ex.Message}");
                }
            }
            catch (Exception ex) 
            {
                await logger.SaveInformationToLog($"DeleteHostingerBackups - {ex.Message}");
            }
        }
    }
}
