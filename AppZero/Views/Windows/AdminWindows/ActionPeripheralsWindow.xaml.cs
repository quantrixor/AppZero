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
        public ActionPeripheralsWindow(Peripherals peripherals)
        {
            InitializeComponent();
            Peripherals = peripherals;


            this.DataContext = this;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCount.Text == "0" || txbDescription.Text == "" || cmbRackNumber.Text == "" || cmbShelfNumber.Text == "")
                    throw new Exception("ВНИМАНИЕ! Пустые значения не допустимы.");

                int numberOfShelves = int.Parse(txbCount.Text); // Получите количество полок, на которых должна быть размещена периферия

                if (Peripherals.ID == 0)
                {
                    Peripherals.DateAdded = DateTime.Now;
                    Peripherals.IDRack = (int)cmbRackNumber.SelectedValue;

                    AppData.db.Peripherals.Add(Peripherals);
                    AppData.db.SaveChanges(); // Сохраните изменения, чтобы получить ID для добавленной периферии

                    int firstShelfIndex = cmbShelfNumber.SelectedIndex; // Индекс выбранной полки
                    var availableShelves = AppData.db.Shelves.Where(s => s.IDRack == Peripherals.IDRack).OrderBy(s => s.ID).ToList(); // Список доступных полок для выбранного стеллажа

                    if (firstShelfIndex + numberOfShelves > availableShelves.Count)
                    {
                        throw new Exception("Недостаточно свободных полок для размещения периферии");
                    }

                    // Проверка занятых полок
                    var occupiedShelfIds = AppData.db.PeripheralShelf.Select(ps => ps.ShelfID).ToList();
                    for (int i = 0; i < numberOfShelves; i++)
                    {
                        int currentShelfId = availableShelves[firstShelfIndex + i].ID;
                        if (occupiedShelfIds.Contains(currentShelfId))
                        {
                            throw new Exception($"Полка {availableShelves[firstShelfIndex + i].Number} уже занята");
                        }
                    }

                    // Сохранение периферии на указанных полках
                    for (int i = 0; i < numberOfShelves; i++)
                    {
                        int currentShelfId = availableShelves[firstShelfIndex + i].ID;
                        PeripheralShelf peripheralShelf = new PeripheralShelf { PeripheralID = Peripherals.ID, ShelfID = currentShelfId };
                        AppData.db.PeripheralShelf.Add(peripheralShelf);
                    }
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
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbRackNumber.ItemsSource = AppData.db.Rack.ToList();
            cmbRackNumber.DisplayMemberPath = "Number";
            cmbRackNumber.SelectedValuePath = "ID";
        }
    }
}
