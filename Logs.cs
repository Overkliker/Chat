using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    internal class Logs
    {
        private static string pathToFile = Path.Combine(Directory.GetCurrentDirectory() + "\\..\\..\\..\\logs.txt");

        public static void write(string message)
        {
            File.AppendAllText(pathToFile, message);
        }
        public static List<string> read()
        {
            Parser parseString = new Parser(File.ReadAllText(pathToFile));
            return parseString.getSplitMessages();
        }
        public static void clear()
        {
            File.WriteAllText(pathToFile, "");
        }
    }
}
