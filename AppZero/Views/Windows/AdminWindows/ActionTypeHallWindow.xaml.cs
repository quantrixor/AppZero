using AppZero.Context;
using AppZero.Model;
using AppZero.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppZero.Views.Windows.AdminWindows
{
    /// <summary>
    /// Interaction logic for ActionTypeHallWindow.xaml
    /// </summary>
    public partial class ActionTypeHallWindow : Window
    {
        public TypeHall selectedTypeHall = null;
        public ObservableCollection<SubtypeHall> subtypeHalls { get; set; }
        public ObservableCollection<TypeHall> typeHalls { get; set; }
        public ActionTypeHallWindow()
        {
            InitializeComponent();
            typeHalls = new ObservableCollection<TypeHall>();
            subtypeHalls = new ObservableCollection<SubtypeHall>();
            LoadData();
            GetSubtypeHall();
            this.DataContext = this;
            listSubtypeHallView.PreviewMouseDown += ListSubtypeHallView_PreviewMouseDown;
            listHallTypeView.PreviewMouseDown += ListHallTypeView_PreviewMouseDown;
        }

        private void ListHallTypeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

            if (item != null && item.IsSelected)
            {
                // Это повторный клик на уже выбранный элемент
                ((ListView)sender).SelectedItem = null;
                selectedTypeHall = null;
                txbHallTypeName.Clear();
                lblTypeHall.Content = "Выберите тип";
                e.Handled = true;
            }
        }

        private void ListSubtypeHallView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

            if (item != null && item.IsSelected)
            {
                // Это повторный клик на уже выбранный элемент
                ((ListView)sender).SelectedItem = null;
                selectedSubtypeItem = null;
                txbSubtypeHallTitle.Clear();
                e.Handled = true;
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }


        private void RemoveTypeHall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedTypeHall = listHallTypeView.SelectedItem as TypeHall;
                if (selectedTypeHall != null)
                {
                    AppData.db.TypeHall.Remove(selectedTypeHall);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные успешно удалены из базу данных!", "Успешно.", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show($"Выберите тип для удаления из базы данных.", "Выберите тип!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch (DbEntityValidationException ex)
            {
                CatchException.DisplayValidationErrors(ex);
            }
            finally
            {
                txbSubtypeHallTitle.Clear();
            }
        }
        private void listHallTypeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedTypeHall = listHallTypeView.SelectedItem as TypeHall;
                if (selectedTypeHall != null)
                {
                    txbHallTypeName.Text = selectedTypeHall.Titiel;
                    lblTypeHall.Content = selectedTypeHall.Titiel;
                    GetSubtypeHall();
                }
            }
            catch (DbEntityValidationException ex)
            {
                CatchException.DisplayValidationErrors(ex);
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

                if (AppData.db.TypeHall.Count(item => item.Titiel == txbHallTypeName.Text) > 0)
                {
                    MessageBox.Show($"Внимание, данные {txbHallTypeName.Text} повторяются. Значения данных типа должны быть уникальными.", "Дубликаты недопустимы!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedTypeHall == null)
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
            finally
            {
                txbHallTypeName.Clear();
                selectedTypeHall = null;
                lblTypeHall.Content = "Выберите тип";
            }
        }
        private void LoadData()
        {
            typeHalls.Clear();
            var typeHallsCollection = AppData.db.TypeHall.ToList();
            foreach (var item in typeHallsCollection)
            {
                typeHalls.Add(item);
            }
        }
        private void GetSubtypeHall()
        {
            subtypeHalls.Clear();
            if (selectedTypeHall == null)
            {
                subtypeHalls.Clear();
                return;
            }
            var subtypeHallsCollection = AppData.db.SubtypeHall.Where(item => item.IDTypeHall == selectedTypeHall.ID).ToList();
            foreach(var item in subtypeHallsCollection)
            {
                subtypeHalls.Add(item);
            }
        }

        private void RemoveSubtypeHall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedSubtypeItem != null)
                {
                    AppData.db.SubtypeHall.Remove(selectedSubtypeItem);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные успешно удалены из базы данных!", "Сохранено.", MessageBoxButton.OK, MessageBoxImage.Information);
                    GetSubtypeHall();
                }
                else
                {
                    MessageBox.Show($"Выберите тип для удаления из базы данных.", "Выберите тип!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

            }
            catch (DbEntityValidationException ex)
            {
                CatchException.DisplayValidationErrors(ex);
            }
            finally
            {
                txbSubtypeHallTitle.Clear();
            }
        }
        private SubtypeHall selectedSubtypeItem = null;
        private void btnSaveSubtypeHall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txbSubtypeHallTitle.Text))
                {
                    MessageBox.Show("Укажите название подтипа зала!", "Пустые значения недопустимы.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (AppData.db.SubtypeHall.Count(item => item.Title == txbSubtypeHallTitle.Text) > 0)
                {
                    MessageBox.Show("Внимание! Такой подтип уже существует!", "Дублирование данных недопустимо.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedTypeHall == null)
                {
                    MessageBox.Show("Внимание! Вы не выбрали тип!", "Выберите тип.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedSubtypeItem == null)
                {
                    SubtypeHall subtypeHall = new SubtypeHall
                    {
                        Title = txbSubtypeHallTitle.Text,
                        IDTypeHall = selectedTypeHall.ID
                    };
                    AppData.db.SubtypeHall.Add(subtypeHall);
                }
                else
                {
                    selectedSubtypeItem.Title = txbSubtypeHallTitle.Text;
                }
                AppData.db.SaveChanges();
                MessageBox.Show("Данные успешно сохранены в базу данных!", "Сохранено.", MessageBoxButton.OK, MessageBoxImage.Information);
                GetSubtypeHall();
            }
            catch (DbEntityValidationException ex)
            {
                CatchException.DisplayValidationErrors(ex);
            }
            finally
            {
                txbSubtypeHallTitle.Clear();
            }
        }

        private object lastSelectedItem = null;

        private void listSubtypeHallView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.SelectedItem == null)
            {
                // Выбор элемента снят
                lastSelectedItem = null;
            }
            else if (listView.SelectedItem == lastSelectedItem)
            {
                // Пользователь нажал на уже выбранный элемент, снимаем выбор
                listView.SelectedItem = null;
                lastSelectedItem = null;
                txbSubtypeHallTitle.Clear();
            }
            else
            {
                // Новый элемент выбран
                lastSelectedItem = listView.SelectedItem;
                selectedSubtypeItem = (SubtypeHall)listView.SelectedItem;
                if (selectedSubtypeItem != null)
                {
                    txbSubtypeHallTitle.Text = selectedSubtypeItem.Title;
                }
            }
        }
    }
}
