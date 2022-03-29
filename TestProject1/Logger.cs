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
        private string filePath;

        public Logger(string fileName)
        {
            filePath = @".\" + fileName;
        }

        public void Log(int stepNumber, string pageTitle, string pageUrl, string message = "")
        {
            File.AppendAllText(filePath, $"Шаг {stepNumber}, название статьи: {pageTitle}, ссылка: {pageUrl} {message}" + "\n");
        }
    }
}
