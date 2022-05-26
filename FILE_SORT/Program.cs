using FILE_SORT.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace APP_CONFIG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"\r\n==========================================================================================");
            Console.WriteLine($"{Assembly.GetEntryAssembly().GetName().Name} - Version {Assembly.GetEntryAssembly().GetName().Version}");
            Console.WriteLine($"==========================================================================================\r\n");

            IConfiguration configuration = ConfigurationLoad();

            (string fileIn, string fileOut, string sortOrder) fileToSort = LoadFileToSort(configuration, 0);
            FileSorter.FileSort(fileToSort);
        }

        static IConfiguration ConfigurationLoad()
        {
            // Get appsettings.json config.
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
        
        static (string, string, string) LoadFileToSort(IConfiguration configuration, int index)
        {
            var filePayload = configuration.GetSection("FileGroup")
                     .GetChildren()
                     .ToList()
                     .Select(x => new
                     {
                         fileIn = x.GetValue<string>("Input"),
                         fileOut = x.GetValue<string>("Output"),
                         sortOrder = x.GetValue<string>("SortOrder")
                     });

            (string fileIn, string fileOut, string sortOrder) result = (null, null, null);

            // Is there a matching item?
            if (filePayload.Count() > index)
            {
               result.fileIn = filePayload.ElementAt(index).fileIn;
               result.fileOut = filePayload.ElementAt(index).fileOut;
               result.sortOrder = filePayload.ElementAt(index).sortOrder;
            }

            return result;
        }
    }
}
