using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldBackupsAutoDelete.Logs
{
    public class Logger
    {
        public static async Task SaveInformationToLog(string information)
        {
            try
            {
                string directoryName =  "logs";
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                string fileName = Path.Combine(directoryName, $"{DateTime.Now:yyyy-MM-dd}-log.txt");
                using StreamWriter sw = new(fileName, true);
                await sw.WriteLineAsync($"{DateTime.Now} {information}");
                sw.Close();
            }
            catch (Exception ex)
            {
                await Task.Delay(1000);
                await SaveInformationToLog(information);
            }
        }
    }
}
