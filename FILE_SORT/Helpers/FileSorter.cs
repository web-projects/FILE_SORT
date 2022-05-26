using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FILE_SORT.Helpers
{
    public static class FileSorter
    {
        public static bool FileSort(List<Tuple<string, string, string>> fileToSort)
        {
            bool result = false;

            try
            {
                string exeLocation = Assembly.GetExecutingAssembly().GetName().CodeBase;
                UriBuilder uri = new UriBuilder(exeLocation);
                string targetDir = Path.Combine(Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)), "in");
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                foreach ((string fileIn, string fileOut, string sortOrder) in fileToSort)
                {
                    string fileInPath = Path.Combine(targetDir, fileIn);

                    if (File.Exists(fileInPath))
                    {
                        string[] logFile = File.ReadAllLines(fileInPath);
                        List<string> logList = new List<string>(logFile);
                        logList.Sort(StringComparer.OrdinalIgnoreCase);
                        switch (sortOrder)
                        {
                            //case "ascending":
                            //    logList.Sort(StringComparer.OrdinalIgnoreCase);
                            //    break;
                            case "descending":
                            logList.Reverse();
                            logList.Sort(StringComparer.OrdinalIgnoreCase);
                            break;
                        }
                        string fileOutPath = Path.Combine(targetDir, fileOut);
                        File.WriteAllLines(fileOutPath, logList);
                        result = true;
                    }
                    else
                    {
                        Console.WriteLine($"FILE [{fileInPath}] - not found!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in fileReader={ex.Message}");
            }
            return result;
        }
    }
}
