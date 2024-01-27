using AppZero.Context;
using AppZero.Model;
using AppZero.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            try
            {
                if (spareParts != null)
                {
                    // Задать контекст данных окна для привязки
                    this.DataContext = spareParts;

                    txbCount.Text = SpareParts.Count.ToString();
                    txbDescription.Text = SpareParts.Description;

                    // Инициализация ComboBox для типов склада
                    cmbWarehouseType.ItemsSource = AppData.db.WarehouseType.ToList();
                    cmbWarehouseType.DisplayMemberPath = "Title"; // Предполагая, что у WarehouseType есть свойство "Title"
                    cmbWarehouseType.SelectedValuePath = "ID";
                    cmbWarehouseType.SelectedValue = spareParts.IDTypeWarehouse; // Установить выбранный тип склада

                    // Инициализация ComboBox для подтипов склада
                    cmbSubType.ItemsSource = AppData.db.SubtypeWarehouseType
                        .Where(item => item.WarehouseTypeId == spareParts.IDTypeWarehouse)
                        .ToList();
                    cmbSubType.DisplayMemberPath = "Title"; // Предполагая, что у SubtypeWarehouseType есть свойство "Title"
                    cmbSubType.SelectedValuePath = "ID";
                    cmbSubType.SelectedValue = spareParts.IDSubtypeWarehouse; // Установить выбранный подтип склада

                    // Инициализация ComboBox для стеллажей
                    cmbRackNumber.ItemsSource = AppData.db.Rack.ToList();
                    cmbRackNumber.DisplayMemberPath = "Number"; // Предполагая, что у Rack есть свойство "Number"
                    cmbRackNumber.SelectedValuePath = "ID";
                    cmbRackNumber.SelectedValue = spareParts.IDRack; // Установить выбранный стеллаж

                    var occupiedShelf = AppData.db.SparePartsShelves
                        .Where(sps => sps.IDSpareParts == spareParts.ID)
                        .Select(sps => sps.Shelves)
                        .FirstOrDefault();

                    if (occupiedShelf != null)
                    {
                        cmbShelfNumber.SelectedValue = occupiedShelf.ID;
                    }
                }
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



        }

        // Сохранение данных СКЛАДА
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на пустые значения
                if (string.IsNullOrWhiteSpace(txbCount.Text) ||
                    string.IsNullOrWhiteSpace(txbDescription.Text) ||
                    cmbRackNumber.SelectedItem == null ||
                    cmbWarehouseType.SelectedItem == null ||
                    cmbSubType.SelectedItem == null ||
                    cmbShelfNumber.SelectedItem == null)
                {
                    throw new Exception("ВНИМАНИЕ! Пустые значения не допустимы.");
                }

                int numberOfShelves = int.Parse(txbCount.Text);
                bool isNewSparePart = SpareParts.ID == 0;
                bool shelvesChanged = false;

                // Если SpareParts уже существуют (редактирование), проверяем, было ли изменение стеллажа или полок
                if (!isNewSparePart)
                {
                    var originalShelves = AppData.db.SparePartsShelves
                        .Where(sps => sps.IDSpareParts == SpareParts.ID)
                        .Select(sps => sps.IDShelf)
                        .ToList();

                    var newShelfIds = GetSelectedShelfIds(cmbShelfNumber, numberOfShelves);
                    shelvesChanged = !newShelfIds.SequenceEqual(originalShelves);
                }
                if(!isNewSparePart && !shelvesChanged)
                {
                    SpareParts.IDTypeWarehouse = ReturnIDObject.ReturnWarehouseType(cmbWarehouseType) ?? 0;
                    SpareParts.IDSubtypeWarehouse = ReturnIDObject.ReturnSubWarehouseType(cmbSubType) ?? 0;
                    SpareParts.Description = txbDescription.Text;

                }
                // Если было изменение или это новая запись, обновляем информацию о SpareParts
                if (shelvesChanged || isNewSparePart)
                {
                    SpareParts.DateAdded = DateTime.Today;
                    SpareParts.IDRack = ReturnIDObject.ReturnRackID(cmbRackNumber) ?? 0;
                    SpareParts.IDTypeWarehouse = ReturnIDObject.ReturnWarehouseType(cmbWarehouseType) ?? 0;
                    SpareParts.IDSubtypeWarehouse = ReturnIDObject.ReturnSubWarehouseType(cmbSubType) ?? 0;
                    SpareParts.Count = numberOfShelves;
                    SpareParts.Description = txbDescription.Text;

                    if (shelvesChanged)
                    {
                        // Удаляем все старые связи с полками
                        var existingShelves = AppData.db.SparePartsShelves.Where(sps => sps.IDSpareParts == SpareParts.ID).ToList();
                        AppData.db.SparePartsShelves.RemoveRange(existingShelves);
                    }

                    if (isNewSparePart)
                    {
                        // Добавляем новую запчасть
                        AppData.db.SpareParts.Add(SpareParts);
                    }

                    // Сохраняем изменения для получения ID новой запчасти или обновления существующей
                    AppData.db.SaveChanges();

                    // Добавляем новые связи с полками
                    foreach (var shelfId in GetSelectedShelfIds(cmbShelfNumber, numberOfShelves))
                    {
                        var newShelf = new SparePartsShelves { IDSpareParts = SpareParts.ID, IDShelf = shelfId };
                        AppData.db.SparePartsShelves.Add(newShelf);
                    }
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
        private List<int> GetSelectedShelfIds(ComboBox cmbShelfNumber, int numberOfShelves)
        {
            var firstShelfIndex = cmbShelfNumber.SelectedIndex;
            var availableShelves = AppData.db.Shelves
                .Where(s => s.IDRack == ((Rack)cmbRackNumber.SelectedItem).ID)
                .OrderBy(s => s.ID)
                .Skip(firstShelfIndex)
                .Take(numberOfShelves)
                .Select(s => s.ID)
                .ToList();

            if (availableShelves.Count < numberOfShelves)
            {
                throw new Exception("Недостаточно свободных полок.");
            }

            return availableShelves;
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
