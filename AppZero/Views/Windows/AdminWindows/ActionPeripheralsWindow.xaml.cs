using AppZero.Context;
using AppZero.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppZero.Views.Windows.AdminWindows
{
    /// <summary>
    /// Логика взаимодействия для ActionPeripheralsWindow.xaml
    /// </summary>
    public partial class ActionPeripheralsWindow : Window
    {
        public List<Rack> GetRack { get; set; }
        public Peripherals Peripherals { get; set; }

        public List<TypeHall> typeHalls { get; set; }

        public ActionPeripheralsWindow(Peripherals peripherals)
        {
            InitializeComponent();
            Peripherals = peripherals;
            typeHalls = AppData.db.TypeHall.ToList();
            this.DataContext = this;
        }
        // Управление данными ЗАЛОВ
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на пустые значения
                if (string.IsNullOrWhiteSpace(txbCount.Text) ||
                    string.IsNullOrWhiteSpace(txbDescription.Text) ||
                    cmbRackNumber.SelectedItem == null ||
                    cmbShelfNumber.SelectedItem == null ||
                    cmbHallType.SelectedItem == null)
                {
                    throw new Exception("ВНИМАНИЕ! Пустые значения не допустимы.");
                }

                int numberOfShelves = int.Parse(txbCount.Text);
                bool isNewPeripherals = Peripherals.ID == 0;

                // Обновляем данные Peripherals
                Peripherals.DateAdded = DateTime.Now;
                Peripherals.IDRack = (int)cmbRackNumber.SelectedValue;
                Peripherals.Description = txbDescription.Text;
                // Обновление других свойств Peripherals...

                if (isNewPeripherals)
                {
                    // Добавляем новую периферию, если это создание
                    AppData.db.Peripherals.Add(Peripherals);
                }
                else
                {
                    // Обновляем существующую периферию, если это редактирование
                    var oldShelves = AppData.db.PeripheralShelf.Where(ps => ps.PeripheralID == Peripherals.ID).ToList();
                    AppData.db.PeripheralShelf.RemoveRange(oldShelves);
                }

                // Сохраняем изменения для получения ID новой периферии или обновления существующей
                AppData.db.SaveChanges();

                // Создание новых записей для PeripheralShelf
                var selectedShelves = GetSelectedShelves(cmbShelfNumber, numberOfShelves, Peripherals.IDRack);
                foreach (var shelfId in selectedShelves)
                {
                    AppData.db.PeripheralShelf.Add(new PeripheralShelf { PeripheralID = Peripherals.ID, ShelfID = shelfId });
                }

                // Сохраняем все изменения в базе данных
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены в базе данных!", "Операция прошла успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Вспомогательный метод для получения ID выбранных полок
        private List<int> GetSelectedShelves(ComboBox cmbShelfNumber, int numberOfShelves, int rackId)
        {
            int firstShelfIndex = cmbShelfNumber.SelectedIndex;
            return AppData.db.Shelves
                .Where(s => s.IDRack == rackId)
                .OrderBy(s => s.ID)
                .Skip(firstShelfIndex)
                .Take(numberOfShelves)
                .Select(s => s.ID)
                .ToList();
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Запрещаем вводить всё, кроме перечисленных цифр
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "1234567890".IndexOf(e.Text) < 0;
        }
        private void cmbRackNumber_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbRackNumber.SelectedValue != null)
            {
                int selectedRackId = (int)cmbRackNumber.SelectedValue;
                var shelvesList = AppData.db.Shelves.Where(s => s.IDRack == selectedRackId).ToList();
                cmbShelfNumber.ItemsSource = shelvesList;
                cmbShelfNumber.DisplayMemberPath = "Number";
                cmbShelfNumber.SelectedValuePath = "ID";

                // Если редактируется существующий объект, установить текущую полку
                if (Peripherals != null && Peripherals.ID > 0)
                {
                    cmbShelfNumber.SelectedItem = shelvesList.FirstOrDefault(s => s.PeripheralShelf.Any(ps => ps.PeripheralID == Peripherals.ID));
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbRackNumber.ItemsSource = AppData.db.Rack.ToList();
            cmbRackNumber.DisplayMemberPath = "Number";
            cmbRackNumber.SelectedValuePath = "ID";
            // Если редактируется существующий объект Peripherals
            if (Peripherals != null && Peripherals.ID > 0)
            {
                // Установить выбранный стеллаж
                cmbRackNumber.SelectedItem = AppData.db.Rack.FirstOrDefault(r => r.ID == Peripherals.IDRack);

                // Установить полки, соответствующие выбранному стеллажу
                var shelves = AppData.db.Shelves.Where(s => s.IDRack == Peripherals.IDRack).ToList();
                cmbShelfNumber.ItemsSource = shelves;
                cmbShelfNumber.DisplayMemberPath = "Number";
                cmbShelfNumber.SelectedValuePath = "ID";

                // Установить выбранную полку
                cmbShelfNumber.SelectedItem = shelves.FirstOrDefault(s => s.PeripheralShelf.Any(ps => ps.PeripheralID == Peripherals.ID));

            }
        }
    }
}
