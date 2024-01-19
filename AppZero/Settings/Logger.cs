using System;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AppZero.Settings
{
    public class Logger
    {
        private readonly string logFilePath;

        public Logger()
        {
            // Пример пути к файлу лога в папке с приложением. Можно использовать и другой путь.
            logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog.txt");
        }

        public async Task LogErrorAsync(string message)
        {
            try
            {
                // Используем StreamWriter для асинхронной записи в файл.
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    await sw.WriteLineAsync($"{DateTime.Now} - {message}");
                }
            }
            catch (Exception ex)
            {
                // Обрабатываем исключения, возникающие при попытке записи в файл лога.
                // Возможно, стоит использовать здесь другой метод логирования, например, Windows Event Log.
                Console.WriteLine("An error occurred while logging: " + ex.Message);
            }
        }
    }

}
