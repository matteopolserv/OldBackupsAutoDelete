using Microsoft.Extensions.Hosting;
using OldBackupsAutoDelete.Contants;
using OldBackupsAutoDelete.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldBackupsAutoDelete
{
    internal class Worker() : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                await Logger.SaveInformationToLog($"Worker - początek pętli while");
                while (DateTime.Now.Hour != 3)
                {
                    await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
                }
                try
                {
                    await Logger.SaveInformationToLog($"Worker - usuwanie plików i katalogów");
                    foreach (string path in PathsList.PathList)
                    {
                        try
                        {
                            await Logger.SaveInformationToLog($"Worker -przetwarzanie {path}");
                            await SearchAndDeleteFiles.DeleteBackups(path);
                        }
                        catch (Exception ex) 
                        {
                            await Logger.SaveInformationToLog($"Worker - pętla while - {ex.Message}");
                        }
                    }
                    await Logger.SaveInformationToLog($"Worker - pliki i katalogi usunięte");
                }
                catch (Exception ex) 
                {
                    await Logger.SaveInformationToLog($"Worker - pętla foreach - {ex.Message}");
                }
                await Task.Delay(TimeSpan.FromMinutes(61));
            }
        }
    }
}
