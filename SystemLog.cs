using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class SystemLog
    {
        public class Log
        {
            public DateTime TimeStamp { get; protected set; }
            public string Message { get; protected set; }

            public Log(string message)
            {
                TimeStamp = DateTime.Now;
                Message = message;
            }
        }

        protected static List<Log> LogList { get; set; } = new List<Log>();

        public static void Add(Log log)
        {
            LogList.Add(log);
        }
        public static void Add(string message)
        {
            LogList.Add(new Log(message));
        }
        public static void Add(Exception e)
        {
            LogList.Add(new Log(e.Message));
        }

        public static void Dump()
        {
            List<Log> sortedList = LogList.Sort((log1, log2) { log1.TimeStamp > log2.TimeStamp; } );
            using (StreamWriter writer = new StreamWriter("log.txt"))
            {
                foreach(Log l in sortedList)
                {
                    wrtier.WriteLine($"{l.TimeStamp} : {l.Message}");
                }
            }
        }
    }
}