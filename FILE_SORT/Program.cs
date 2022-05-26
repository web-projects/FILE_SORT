using FILE_SORT.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APP_CONFIG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"\r\n==========================================================================================");
            Console.WriteLine($"{Assembly.GetEntryAssembly().GetName().Name} - Version {Assembly.GetEntryAssembly().GetName().Version}");
            Console.WriteLine($"==========================================================================================\r\n");

            IConfiguration configuration = ConfigurationLoad();

            List<Tuple<string, string, string>> fileToSort = LoadFileToSort(configuration);
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

        static List<Tuple<string, string, string>> LoadFileToSort(IConfiguration configuration)
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

            List<Tuple<string, string, string>> results = new List<Tuple<string, string, string>>();

            if (filePayload.Count() > 0)
            {
                List<string> fileIn = new List<string>();
                List<string> fileOut = new List<string>();
                List<string> sortOrder = new List<string>();

                int index = 0;
                fileIn.AddRange(from value in filePayload
                                select filePayload.ElementAt(index++).fileIn);
                index = 0;
                fileOut.AddRange(from value in filePayload
                                 select filePayload.ElementAt(index++).fileOut);
                index = 0;
                sortOrder.AddRange(from value in filePayload
                                   select filePayload.ElementAt(index++).sortOrder);

                foreach (var combinedOutput in fileIn
                    .Zip(fileOut, (vc, vv) => Tuple.Create(vc, vv))
                    .Zip(sortOrder, (vcvv, o) => Tuple.Create(vcvv.Item1, vcvv.Item2, o)))
                {
                    results.Add(combinedOutput);
                }
            }

            return results;
        }
    }
}
