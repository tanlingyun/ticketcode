using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxSDK.TicketCode.Helpers
{
    internal class Logger<T>
    {
        internal static void LogInformation(string message)
        {
            WriteLog("Information", message);
        }

        internal static void LogError(string message)
        {
            WriteLog("Error", message);
        }

        internal static void LogWarring(string message)
        {
            WriteLog("Warring", message);
        }

        static void WriteLog(string level, string log)
        {
            //格式化
            var text = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{level}][HxSMS][{typeof(T).Name}] {log}" + Environment.NewLine;
            //日志保存目录
            var logdir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!System.IO.Directory.Exists(logdir))
                System.IO.Directory.CreateDirectory(logdir);
            var path = System.IO.Path.Combine(logdir,$"HxSDK-{DateTime.Now.ToString("yyyyMMdd")}.txt");
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite))
            {
                var data = System.Text.Encoding.UTF8.GetBytes(text);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
