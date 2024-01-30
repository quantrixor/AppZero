using AppZero.Context;
using AppZero.Model;
using AppZero.Settings;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppZero.Views.Windows.AdminWindows
{
    /// <summary>
    /// Interaction logic for ActionTypeHallWindow.xaml
    /// </summary>
    public partial class ActionTypeHallWindow : Window
    {
        public TypeHall selectedTypeHall = null;
        public ActionTypeHallWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void RemoveTypeHall_Click(object sender, RoutedEventArgs e)
        {
            selectedTypeHall = listHallTypeView.SelectedItem as TypeHall;
            if (selectedTypeHall != null)
            {
                AppData.db.TypeHall.Remove(selectedTypeHall);
                AppData.db.SaveChanges();
                MessageBox.Show("Данные успешно удалены из базу данных!", "Успешно.", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
        }

        private void listHallTypeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTypeHall = listHallTypeView.SelectedItem as TypeHall;
            if(selectedTypeHall != null)
            {
                txbHallTypeName.Text = selectedTypeHall.Titiel;
            }
        }

        private void btnSaveTypeHall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txbHallTypeName.Text))
                {
                    MessageBox.Show("Пустое значение недопустимо!", "Заполните поле!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if(AppData.db.TypeHall.Count(item => item.Titiel == txbHallTypeName.Text) > 0)
                {
                    MessageBox.Show($"Внимание, данные {txbHallTypeName.Text} повторяются. Значения данных типа должны быть уникальными.", "Дубликаты недопустимы!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if(selectedTypeHall == null)
                {
                    TypeHall type = new TypeHall
                    {
                        Titiel = txbHallTypeName.Text
                    };
                    AppData.db.TypeHall.Add(type);
                }
                else
                {
                    selectedTypeHall.Titiel = txbHallTypeName.Text;
                }
                AppData.db.SaveChanges();
                MessageBox.Show("Данные успешно сохранены в базу данных!", "Успешно.", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();

            }
            catch (DbEntityValidationException ex)
            {
                CatchException.DisplayValidationErrors(ex);
            }
        }
        private void LoadData()
        {
            listHallTypeView.ItemsSource = AppData.db.TypeHall.ToList();
        }
    }
}
