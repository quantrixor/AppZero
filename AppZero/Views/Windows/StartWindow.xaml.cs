using AppZero.Model;
using AppZero.Settings;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace AppZero.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var progressIndicator = new Progress<string>(UpdateLoadingLabel);

            // Инициализация приложения и проверка подключения к базе данных
            bool isInitialized = await InitializeApplicationAsync(progressIndicator);

            // Если инициализация прошла успешно, открываем главное окно
            if (isInitialized)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close(); // Закрываем текущее окно
            }
            else
            {
                // Можно показать сообщение пользователю или логировать ошибку
                MessageBox.Show("Unable to initialize the application. Please check the log for more details.", "WARNIGN! NOT CONNECTION!", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
            }
        }

        private void UpdateLoadingLabel(string status)
        {
            LoadingLabel.Text = status;
        }

        private async Task<bool> InitializeApplicationAsync(IProgress<string> progress)
        {
            progress.Report("Initializing...");

            // Здесь код для инициализации
            await Task.Delay(1000); // Убрать эту строку, когда добавите реальную логику

            progress.Report("Loading modules...");
            // Здесь ваш код для загрузки модулей
            await Task.Delay(1000); // Убрать эту строку, когда добавите реальную логику

            progress.Report("Connecting to database...");
            var isDbConnected = await TestDatabaseConnectionAsync();
            if (!isDbConnected)
            {
                progress.Report("Cannot connect to the database!");
                // Отображаем сообщение об ошибке или закрываем приложение
                return false; // Прерываем инициализацию, если нет подключения к базе данных
            }

            // Если соединение с базой данных успешно установлено, продолжаем инициализацию
            progress.Report("Database connection established...");

            progress.Report("Finalizing...");
            // Здесь ваш код для финализации инициализации
            await Task.Delay(1000); // Убрать эту строку, когда добавите реальную логику

            progress.Report("Done!");
            return true;
        }

        private async Task<bool> TestDatabaseConnectionAsync()
        {
            var logger = new Logger();
            try
            {
                using (var context = new dbLocalEntities())
                {
                    // Асинхронно открыть соединение с базой данных
                    await context.Database.Connection.OpenAsync();

                    context.Database.Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений, связанных с подключением к базе данных
                await logger.LogErrorAsync("Ошибка подключения к базе данных: " + ex.Message);
                return false;
            }
        }

    }
}
