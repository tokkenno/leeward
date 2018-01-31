using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Leeward.Utils
{
    internal class Logger
    {
        public enum Level { Fatal, Error, Warning, Info, Debug, Trace };

        private static Dictionary<String, Logger> _loggers = new Dictionary<String, Logger>();

        public static Logger Get(Type type)
        {
            return Get(type.Name);
        }

        public static Logger Get(String name)
        {
            if (_loggers.ContainsKey(name)) return _loggers[name];
            else
            {
                Logger newLogger = new Logger(name);
                _loggers.Add(name, newLogger);
                return newLogger;
            }
        }

        private String _name;

        private Logger(String name)
        {
            this._name = name;
        }

        public void Fatal(String message) { Log(Level.Fatal, message); }
        public void Error(String message) { Log(Level.Error, message); }
        public void Warning(String message) { Log(Level.Warning, message); }
        public void Info(String message) { Log(Level.Info, message); }
        public void Debug(String message) { Log(Level.Debug, message); }
        public void Trace(String message) { Log(Level.Trace, message); }

        public void Log(Level lvl, String message)
        {
            Task.Run(() =>
            {
                String hour = DateTime.UtcNow.ToString("H:mm:ss.fff");
                switch(lvl)
                {
                    case Level.Fatal:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{hour}][ FATAL ][{_name}] {message}");
                        break;
                    case Level.Error:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"[{hour}][ ERROR ][{_name}] {message}");
                        break;
                    case Level.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[{hour}][WARNING][{_name}] {message}");
                        break;
                    case Level.Info:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[{hour}][ INFO  ][{_name}] {message}");
                        break;
                    case Level.Debug:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[{hour}][ DEBUG ][{_name}] {message}");
                        break;
                    default:  case Level.Trace:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"[{hour}][ TRACE ][{_name}] {message}");
                        break;
                }

                Console.ForegroundColor = ConsoleColor.White;
            });
        }
    }
}
