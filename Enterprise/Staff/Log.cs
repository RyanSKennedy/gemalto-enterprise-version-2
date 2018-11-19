﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace MyLogClass
{
    public class Log
    {
        private static object sync = new object();
        public static void Write(string eventString)
        {
            try {
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog);//создаём директорию для логов если нужно
                string fileName = Path.Combine(pathToLog, "app.log");
                string fullText = string.Format("[{0:dd.MM.yyyy  HH:mm:ss,fff}]  {1}\r\n", DateTime.Now, eventString);
                lock (sync) {
                    File.AppendAllText(fileName, fullText);
                }
            } catch {
                //перехватываем всё иничего не делаем
            }
        }
    }
}