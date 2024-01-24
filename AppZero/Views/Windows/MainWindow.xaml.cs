using AppZero.Model;
using AppZero.Views.Pages;
using AppZero.Views.Pages.AdminPages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AppZero
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new AuthorizationPage());

        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтвердите.", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                mainFrame.Navigate(new AuthorizationPage());
            }
        }

        private void mainFrame_ContentRendered(object sender, EventArgs e)
        {
            btnSignOut.Visibility = mainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ChangeTheme(Uri themeUri)
        {
            // Пытаемся найти словарь с таким же источником, как у themeUri.
            var existingDict = Application.Current.Resources.MergedDictionaries
                                        .FirstOrDefault(d => d.Source == themeUri);

            // Если словарь уже загружен, ничего не делаем.
            if (existingDict != null)
                return;

            // Создаем новый словарь ресурсов для загрузки.
            var dict = new ResourceDictionary { Source = themeUri };

            // Удаляем все текущие темы из MergedDictionaries.
            Application.Current.Resources.MergedDictionaries.Clear();

            // Добавляем новый словарь ресурсов.
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
       

        private void ThemeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            // Переключение на темную тему.
            ChangeTheme(new Uri("pack://application:,,,/Themes/DarkTheme.xaml"));
        }

        private void ThemeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            // Переключение на светлую тему.
            ChangeTheme(new Uri("pack://application:,,,/Themes/LightTheme.xaml"));
        }

        private void mainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content is Page)
            {
                ((Page)e.Content).KeepAlive = false;
            }
        }
    }
}
