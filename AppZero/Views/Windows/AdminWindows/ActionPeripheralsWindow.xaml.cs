using AppZero.Context;
using AppZero.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCount.Text == "0" || txbDescription.Text == "" || cmbRackNumber.SelectedValue == null || cmbShelfNumber.SelectedValue == null || cmbHallType.SelectedValue == null)
                    throw new Exception("ВНИМАНИЕ! Пустые значения не допустимы.");

                int numberOfShelves = int.Parse(txbCount.Text);
                Peripherals.DateAdded = DateTime.Now;
                Peripherals.IDRack = (int)cmbRackNumber.SelectedValue;

                // Если это редактирование, необходимо удалить старые записи с полок
                if (Peripherals.ID != 0)
                {
                    var oldShelves = AppData.db.PeripheralShelf.Where(ps => ps.PeripheralID == Peripherals.ID).ToList();
                    foreach (var oldShelf in oldShelves)
                    {
                        AppData.db.PeripheralShelf.Remove(oldShelf);
                    }
                    AppData.db.SaveChanges(); // Сохраняем изменения сразу после удаления
                }

                // Создание новых записей для PeripheralShelf
                int firstShelfIndex = cmbShelfNumber.SelectedIndex;
                var availableShelves = AppData.db.Shelves.Where(s => s.IDRack == Peripherals.IDRack).OrderBy(s => s.ID).ToList();

                if (firstShelfIndex + numberOfShelves > availableShelves.Count)
                {
                    throw new Exception("Недостаточно свободных полок для размещения периферии");
                }

                for (int i = 0; i < numberOfShelves; i++)
                {
                    int currentShelfId = availableShelves[firstShelfIndex + i].ID;
                    PeripheralShelf peripheralShelf = new PeripheralShelf { PeripheralID = Peripherals.ID, ShelfID = currentShelfId };
                    AppData.db.PeripheralShelf.Add(peripheralShelf);
                }

                // Если это создание новой периферии, добавить ее в базу данных
                if (Peripherals.ID == 0)
                {
                    AppData.db.Peripherals.Add(Peripherals);
                }

                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены в базе данных!", "Операция прошла успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
