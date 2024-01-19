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
    /// Логика взаимодействия для ActionSparePartsWindow.xaml
    /// </summary>
    public partial class ActionSparePartsWindow : Window
    {
        public SpareParts SpareParts { get; set; }
        public List<Peripherals> TypeObjects { get; set; }
        public ActionSparePartsWindow(SpareParts spareParts)
        {
            InitializeComponent();
            this.SpareParts = spareParts;
            TypeObjects = AppData.db.Peripherals.ToList();

            this.DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCount.Text == "0" || txbDescription.Text == "" || cmbRackNumber.Text == "" || cmbShelfNumber.Text == "" || cmbTypeObject.Text == "")
                    throw new Exception("ВНИМАНИЕ! Пустые значения не допустимы.");

                int numberOfShelves = int.Parse(txbCount.Text); // Получите количество полок, на которых должна быть размещена периферия

                if (SpareParts.ID == 0)
                {
                    SpareParts.DateAdded = DateTime.Now;
                    SpareParts.IDRack = (int)cmbRackNumber.SelectedValue; // Задайте значение IDRack для SpareParts
                    AppData.db.SpareParts.Add(SpareParts);

                    int firstShelfIndex = cmbShelfNumber.SelectedIndex; // Индекс выбранной полки
                    var availableShelves = AppData.db.Shelves.Where(s => s.IDRack == SpareParts.IDRack).OrderBy(s => s.ID).ToList(); // Список доступных полок для выбранного стеллажа

                    if (firstShelfIndex + numberOfShelves > availableShelves.Count)
                    {
                        throw new Exception("Недостаточно свободных полок для размещения запчастей");
                    }

                    // Проверка занятых полок
                    var occupiedShelfIds = AppData.db.SparePartsShelves.Select(ps => ps.IDShelf).ToList();
                    for (int i = 0; i < numberOfShelves; i++)
                    {
                        int currentShelfId = availableShelves[firstShelfIndex + i].ID;
                        if (occupiedShelfIds.Contains(currentShelfId))
                        {
                            throw new Exception($"Полка {availableShelves[firstShelfIndex + i].Number} уже занята");
                        }
                    }

                    // Сохранение запчастей на указанных полках
                    for (int i = 0; i < numberOfShelves; i++)
                    {
                        int currentShelfId = availableShelves[firstShelfIndex + i].ID;
                        SparePartsShelves peripheralShelf = new SparePartsShelves { IDSpareParts = SpareParts.ID, IDShelf = currentShelfId };
                        AppData.db.SparePartsShelves.Add(peripheralShelf);
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
