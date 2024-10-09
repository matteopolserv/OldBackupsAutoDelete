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
        public static async Task DeleteBackups(string directoriesBackupPath)
        {
            try
            {                
                DirectoryInfo directoriesBackup = new (directoriesBackupPath);
                try
                {
                    var filesToDelete = directoriesBackup.GetFiles("*.*", SearchOption.AllDirectories)
                                .Where(f => f.CreationTime.AddDays(60) < DateTime.Now ||
                            (f.CreationTime.AddDays(10) < DateTime.Now && f.CreationTime.DayOfWeek is not DayOfWeek.Wednesday)).ToList();
                    await Logger.SaveInformationToLog($"Usuwanie plików. Liczba pików do usunięcia: {filesToDelete.Count}");
                   Parallel.ForEach(filesToDelete, f =>
                    {
                        f.Delete();

                    });
                }
                catch (Exception ex)
                {
                    await Logger.SaveInformationToLog($"DeleteHostingerBackups - Usuwanie plików - {ex.Message}");
                }
                try
                {
                    List<DirectoryInfo> emptyDirectories = directoriesBackup.GetDirectories("*.*", SearchOption.AllDirectories)
                    .Where(d => d.GetFiles("*.*", SearchOption.TopDirectoryOnly).Length == 0 && d.GetDirectories("*.*", SearchOption.TopDirectoryOnly).Length == 0)
                    .ToList();
                    await Logger.SaveInformationToLog($"Usuwanie pusty katalogów. Liczba katalogów do usunięcia: {emptyDirectories.Count}");
                    Parallel.ForEach(emptyDirectories, emptyDirectory =>
                    {
                        emptyDirectory.Delete(false);
                    });
                }
                catch(Exception ex)
                {
                    await Logger.SaveInformationToLog($"DeleteHostingerBackups - Usuwanie katalogów - {ex.Message}");
                }
            }
            catch (Exception ex) 
            {
                await Logger.SaveInformationToLog($"DeleteHostingerBackups - {ex.Message}");
            }
        }
        public static async Task DeleteSSHBackups(string directoriesBackupPath)
        {
            try
            {                
                DirectoryInfo directoriesBackup = new (directoriesBackupPath);
                try
                {
                    var filesToDelete = directoriesBackup.GetFiles("*.*", SearchOption.AllDirectories)
                                .Where(f => f.CreationTime.AddDays(60) < DateTime.Now ||
                            (f.CreationTime.AddDays(10) < DateTime.Now && f.CreationTime.DayOfWeek is not DayOfWeek.Wednesday)).ToList();
                    Parallel.ForEach(filesToDelete, f =>
                    {
                        f.Delete();
                    });
                }
                catch (Exception ex)
                {
                    await Logger.SaveInformationToLog($"DeleteHostingerBackups - Usuwanie plików - {ex.Message}");
                }
                try
                {
                    List<DirectoryInfo> emptyDirectories = directoriesBackup.GetDirectories("*.*", SearchOption.AllDirectories)
                    .Where(d => d.GetFiles("*.*", SearchOption.TopDirectoryOnly).Length == 0 && d.GetDirectories("*.*", SearchOption.TopDirectoryOnly).Length == 0)
                    .ToList();
                    await Logger.SaveInformationToLog($"Usuwanie pusty katalogów. Liczba katalogów do usunięcia: {emptyDirectories.Count}");
                    Parallel.ForEach(emptyDirectories, emptyDirectory =>
                    {
                        emptyDirectory.Delete(false);
                    });
                }
                catch(Exception ex)
                {
                    await Logger.SaveInformationToLog($"DeleteHostingerBackups - Usuwanie katalogów - {ex.Message}");
                }
            }
            catch (Exception ex) 
            {
                await Logger.SaveInformationToLog($"DeleteHostingerBackups - {ex.Message}");
            }
        }
    }
}
