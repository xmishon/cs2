using static System.Console;
using System.IO;
using System;

namespace MyGame
{
    /// <summary>
    /// Мой статический класс с методами для логирования в консоль и в файл
    /// </summary>
    static class Logger
    {
        public static string Filename { get; private set; }

        public static void setFilename(string filename)
        {
            Filename = filename;
        }

        /// <summary>
        /// Выводит строку в консоль
        /// </summary>
        /// <param name="message">Выводимая в консоль строка</param>
        public static void logConsole(string message)
        {
            WriteLine(DateTime.Now.ToString() + " : " + message);
        }

        /// <summary>
        /// Записывает строку в файл
        /// </summary>
        /// <param name="message">Записываемая в файл строка</param>
        public static void logFile(string message)
        {
            File.AppendAllText(Filename, DateTime.Now.ToString() + " : " + message + "\r\n");
        }
    }
}
