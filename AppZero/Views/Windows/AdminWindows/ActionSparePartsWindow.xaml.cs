using AppZero.Context;
using AppZero.Model;
using AppZero.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            cmbWarehouseType.ItemsSource = AppData.db.WarehouseType.ToList();

            this.DataContext = this;

            if(spareParts != null)
            {
                cmbRackNumber.SelectedItem = spareParts.Rack;
                cmbWarehouseType.SelectedItem = spareParts.WarehouseType;
                cmbSubType.SelectedItem = spareParts.SubtypeWarehouseType;
            }
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbCount.Text == "0" || txbDescription.Text == "" || cmbRackNumber.Text == "" || cmbShelfNumber.Text == "")
                    throw new Exception("ВНИМАНИЕ! Пустые значения не допустимы.");

                int numberOfShelves = int.Parse(txbCount.Text); // Получите количество полок, на которых должна быть размещена периферия

                if (SpareParts.ID == 0)
                {
                    SpareParts.DateAdded = DateTime.Today;
                    SpareParts.IDRack = ReturnIDObject.ReturnRackID(cmbRackNumber) ?? 0; // Задайте значение IDRack для SpareParts
                    SpareParts.IDTypeWarehouse = ReturnIDObject.ReturnWarehouseType(cmbWarehouseType) ?? 0;
                    SpareParts.IDSubtypeWarehouse = ReturnIDObject.ReturnSubWarehouseType(cmbSubType) ?? 0;
                    SpareParts.Count = int.Parse(txbCount.Text);
                    SpareParts.Description = txbDescription.Text;
                    
                    AppData.db.SpareParts.Add(SpareParts);

                    int firstShelfIndex = cmbShelfNumber.SelectedIndex; // Индекс выбранной полки
                    var availableShelves = AppData.db.Shelves.Where(s => s.IDRack == SpareParts.IDRack).OrderBy(s => s.ID).ToList(); // Список доступных полок для выбранного стеллажа

                    if (firstShelfIndex + numberOfShelves > availableShelves.Count)
                    {
                        throw new Exception("Недостаточно свободных полок.");
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
                try
                {

                    AppData.db.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            sb.AppendFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            sb.AppendLine();
                        }
                    }
                    MessageBox.Show(sb.ToString(), "Entity Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    var innerException = ex;
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;
                    }
                    MessageBox.Show($"Error: {ex.Message} Inner exception: {innerException.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
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

        private void cmbWarehouseType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItem = cmbWarehouseType.SelectedItem as WarehouseType;
            if (selectedItem != null)
            {
                cmbSubType.ItemsSource = AppData.db.SubtypeWarehouseType.Where(item => item.WarehouseTypeId == selectedItem.ID).ToList();
            }
        }
    }
}
