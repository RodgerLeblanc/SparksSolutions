using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Utf8FileSaver
{
    class Program
    {
        static void Main(string[] args)
        {
            var directoryInfo = new DirectoryInfo(@"../../../../../Website/");
            var extensions = new[] { "*.js", "*.css", "*.html" };
            var fileInfos = extensions.SelectMany(ext => directoryInfo.GetFiles(ext, SearchOption.AllDirectories));

            foreach (var fileInfo in fileInfos)
            {
                string s = File.ReadAllText(fileInfo.FullName);
                File.WriteAllText(fileInfo.FullName, s, Encoding.UTF8);
                Console.WriteLine(fileInfo.FullName);
            }
        }
    }
}