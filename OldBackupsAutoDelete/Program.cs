using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OldBackupsAutoDelete;
using OldBackupsAutoDelete.Logs;


internal class Program
{
    private static void Main(string[] args)
    {        
        
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
       
        builder.Services.AddScoped<Logger>();
        builder.Services.AddScoped<SearchAndDeleteFiles>();
        builder.Services.AddHostedService<Worker>();
        IHost host = builder.Build();
        host.Run();
    }
}