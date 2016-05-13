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
        private static Logger theInstance;
        private static StreamWriter sWriter = null;

        /*
        * Initialization.
        */
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

        /*
        * Creates a logfile if there is none, writes down the errors caught in a .txt file for revision.
        */
        public static void LogException(Exception ex)
        {
            var directoryPath = HttpContext.Current.Server.MapPath("~/Logs/");
            var logFilePath = "coder_logfile.txt";
            var message = string.Format("{0} Time: {1}.{3}For: {2}{3}", ex.Message, DateTime.Now, ex.Source, Environment.NewLine);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (var writer = new StreamWriter(directoryPath + logFilePath, true, Encoding.Default))
            {
                writer.WriteLine(message);
            }
        }
    }
}