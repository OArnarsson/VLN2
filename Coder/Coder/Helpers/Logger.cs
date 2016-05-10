using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text;

namespace Coder.Helpers
{
    public class Logger
    {
        private static Logger theInstance = null;
        private static StreamWriter sWriter = null;

        public static Logger Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new Logger();
                }

                return theInstance;
            }
        }

        public static void LogException(Exception ex)
        {
            string directoryPath = HttpContext.Current.Server.MapPath("~/Logs/");
            string logFilePath = "coder_logfile.txt";
            string message = string.Format("{0} Time: {1}.{3}For: {2}{3}", ex.Message, DateTime.Now, ex.Source, Environment.NewLine);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (StreamWriter writer = new StreamWriter(directoryPath + logFilePath, true, Encoding.Default))
            {
                writer.WriteLine(message);
            }
        }
    }
}