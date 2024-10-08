using Microsoft.Extensions.Hosting;
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
                    await SearchAndDeleteFiles.DeleteHostingerBackups();
                    await Logger.SaveInformationToLog($"Worker - pliki i katalogi usunięte");
                }
                catch (Exception ex) 
                {
                    await Logger.SaveInformationToLog($"Worker - pętla while - {ex.Message}");
                }

            }
        }
    }
}
