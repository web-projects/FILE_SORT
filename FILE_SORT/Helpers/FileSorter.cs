using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FILE_SORT.Helpers
{
    public static class FileSorter
    {
        public static bool FileSort((string fileIn, string fileOut, string sortOrder) fileToSort)
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

                string fileInPath = Path.Combine(targetDir, fileToSort.fileIn);

                if (File.Exists(fileInPath))
                {
                    string[] logFile = File.ReadAllLines(fileInPath);
                    List<string> logList = new List<string>(logFile);
                    logList.Sort(StringComparer.OrdinalIgnoreCase);
                    switch (fileToSort.sortOrder)
                    {
                        //case "ascending":
                        //    logList.Sort(StringComparer.OrdinalIgnoreCase);
                        //    break;
                        case "descending":
                            logList.Reverse();
                            logList.Sort(StringComparer.OrdinalIgnoreCase);
                            break;
                    }
                    string fileOutPath = Path.Combine(targetDir, fileToSort.fileOut);
                    File.WriteAllLines(fileOutPath, logList);
                    result = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception in fileReader={ex.Message}");
            }
            return result;
        }
    }
}
