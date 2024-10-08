using Microsoft.Extensions.Hosting;
using OldBackupsAutoDelete.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldBackupsAutoDelete
{
    internal class Worker(Logger logger, SearchAndDeleteFiles searchAndDelete) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                await logger.SaveInformationToLog($"Worker - początek pętli while");
                while (DateTime.Now.Hour != 3)
                {
                    await Task.Delay(TimeSpan.FromMinutes(15));
                }
                try
                {
                    await logger.SaveInformationToLog($"Worker - usuwanie plików i katalogów");
                    await searchAndDelete.DeleteHostingerBackups();
                    await logger.SaveInformationToLog($"Worker - pliki i katalogi usunięte");
                }
                catch (Exception ex) 
                {
                    await logger.SaveInformationToLog($"Worker - pętla while - {ex.Message}");
                }

            }
        }
    }
}
