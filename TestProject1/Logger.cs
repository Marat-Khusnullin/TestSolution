using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    internal class Logger
    {
        public static void Log(int stepNumber, string pageTitle, string pageUrl, string message = "")
        {
            File.AppendAllText(@".\logs.txt", $"Шаг {stepNumber}, название статьи: {pageTitle}, ссылка: {pageUrl} {message}" + "\n");
        }
    }
}
