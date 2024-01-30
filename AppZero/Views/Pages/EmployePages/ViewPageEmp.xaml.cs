using AppZero.Context;
using AppZero.Model;
using AppZero.Views.Windows.AdminWindows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;

using Word = Microsoft.Office.Interop.Word;

namespace AppZero.Views.Pages.EmployePages
{
    /// <summary>
    /// Логика взаимодействия для ViewPageEmp.xaml
    /// </summary>
    public partial class ViewPageEmp : Page
    {
        public List<SpareParts> SparePartsDestination = new List<SpareParts>();
        public List<Peripherals> PeripheralsDestination = new List<Peripherals>();
        public ViewPageEmp()
        {
            InitializeComponent();
        }
        // Поиск данных по складу
        private void txbSearchDevice_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Поиск по следующим критериям: ID, Номер стеллажа, Номер полки и Количество на складе
                ListDataSpareParts.ItemsSource = AppData.db.SpareParts.Where(item => item.ID.ToString().Contains(txbSearchDevice.Text) ||
                item.Rack.Number.Contains(txbSearchDevice.Text) || item.WarehouseType.Title.Contains(txbSearchDevice.Text) ||
                item.Count.ToString().Contains(txbSearchDevice.Text)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void sortDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ListDataSpareParts.ItemsSource = AppData.db.SpareParts.Where(item => item.DateAdded == sortDate.SelectedDate).ToList();
        }

        // Поиск данных по Залу
        private void txbSearchPeripher_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Поиск по следующим критериям: ID, Номер стеллажа, Номер полки и Количество на складе
                listDataPeripher.ItemsSource = AppData.db.Peripherals.Where(item => item.ID.ToString().Contains(txbSearchPeripher.Text) ||
                        item.Rack.Number.Contains(txbSearchPeripher.Text) ||
                        item.Description.Contains(txbSearchPeripher.Text) ||
                        item.TypeHall.Titiel.Contains(txbSearchPeripher.Text) ||
                        item.Count.ToString().Contains(txbSearchPeripher.Text)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void sortDatePeripher_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            listDataPeripher.ItemsSource = AppData.db.Peripherals.Where(item => item.DateAdded == sortDatePeripher.SelectedDate).ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListDataSpareParts.ItemsSource = AppData.db.SpareParts.ToList();
            listDataPeripher.ItemsSource = AppData.db.Peripherals.ToList();
            FilterTypeHallComboBox.ItemsSource = AppData.db.TypeHall.ToList();
            LoadWarehouseTypes();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            sortDate.SelectedDate = null;
            sortDatePeripher.SelectedDate = null;
            FilterTypeHallComboBox.SelectedItem = null;
            FilterWarehouseType.SelectedItem = null;
            FilterSubypeWarehouse.SelectedItem = null;
            Page_Loaded(null, null);
        }
        private void ExportSparePartsDataPDF()
        {
            var word = new Word.Application();
            try
            {
                var document = word.Documents.Add();
                var paragrah = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragrah.Range;
                var listDataSpareParts = SparePartsDestination;
                var table = document.Tables.Add(tableRange, listDataSpareParts.Count + 1, 8);
                table.Range.Font.Size = 10;
                table.Borders.Enable = 1;
                table.Title = "Данные склада";
                table.Cell(1, 1).Range.Text = "Номер стеллажа";
                table.Cell(1, 2).Range.Text = "Номер полки";
                table.Cell(1, 3).Range.Text = "Описание";
                table.Cell(1, 4).Range.Text = "Тип";
                table.Cell(1, 5).Range.Text = "Подтип";
                table.Cell(1, 6).Range.Text = "Количество";
                table.Cell(1, 7).Range.Text = "Дата";

                int i = 2;
                foreach (var item in listDataSpareParts)
                {
                    table.Cell(i, 1).Range.Text = item.Rack.Number;
                    table.Cell(i, 2).Range.Text = item.ShellRackNumber;
                    table.Cell(i, 3).Range.Text = item.Description;
                    table.Cell(i, 4).Range.Text = item.WarehouseType.Title;
                    table.Cell(i, 5).Range.Text = item.SubtypeWarehouseType.Title;
                    table.Cell(i, 6).Range.Text = item.Count.ToString();
                    table.Cell(i, 7).Range.Text = item.DateAdded.ToString();
                    i++;
                }
                document.SaveAs2($"{Environment.CurrentDirectory}\\EmpData.pdf", Word.WdSaveFormat.wdFormatPDF);
                //document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                MessageBox.Show($"Документ успешно сформирован, расположение: {Environment.CurrentDirectory}\\Data.pdf!", "Документ успешно сформирован.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source + "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
            }
        }
        private void ExportPeripherDataPDF()
        {
            var word = new Word.Application();
            try
            {
                var document = word.Documents.Add();
                var paragrah = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragrah.Range;
                var listDataSpareParts = PeripheralsDestination;
                var table = document.Tables.Add(tableRange, listDataSpareParts.Count + 1, 5);
                table.Range.Font.Size = 10;
                table.Borders.Enable = 1;
                table.Title = "Данные";
                table.Cell(1, 1).Range.Text = "Номер стеллажа";
                table.Cell(1, 2).Range.Text = "Номера полок";
                table.Cell(1, 3).Range.Text = "Описание";
                table.Cell(1, 4).Range.Text = "Количество";
                table.Cell(1, 5).Range.Text = "Дата";

                int i = 2;
                foreach (var item in listDataSpareParts)
                {
                    table.Cell(i, 1).Range.Text = item.Rack.Number;
                    table.Cell(i, 2).Range.Text = item.ShellRackNumberPeripherals;
                    table.Cell(i, 3).Range.Text = item.Description;
                    table.Cell(i, 4).Range.Text = item.Count.ToString();
                    table.Cell(i, 5).Range.Text = item.DateAdded.ToString();
                    i++;
                }
                document.SaveAs2($"{Environment.CurrentDirectory}\\EmpDataPeripher.pdf", Word.WdSaveFormat.wdFormatPDF);
                //document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                document.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
                MessageBox.Show($"Документ успешно сформирован, расположение: {Environment.CurrentDirectory}\\DataPeripher.pdf!", "Документ успешно сформирован.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source + "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                word.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpStartDate.SelectedDate != null && dtpEndDate.SelectedDate != null)
                {
                    SparePartsDestination = AppData.db.SpareParts.Where(item => item.DateAdded >= dtpStartDate.SelectedDate && item.DateAdded <= dtpEndDate.SelectedDate).ToList();
                    ExportSparePartsDataPDF();
                }
                else
                {
                    throw new Exception("Укажите дату!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnPrintPeripher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpStartDatePeripher.SelectedDate != null && dtpEndDatePeripher.SelectedDate != null)
                {
                    PeripheralsDestination = AppData.db.Peripherals.Where(item => item.DateAdded >= dtpStartDatePeripher.SelectedDate && item.DateAdded <= dtpEndDatePeripher.SelectedDate).ToList();
                    ExportPeripherDataPDF();
                }
                else
                {
                    throw new Exception("Укажите дату!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Фильтрация по типу зала
        private void FilterTypeHallComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTypeHall = FilterTypeHallComboBox.SelectedItem as TypeHall;

            if (selectedTypeHall != null)
            {
                var filteredPeripherals = AppData.db.Peripherals
                                                   .Where(p => p.IDTypeHall == selectedTypeHall.ID)
                                                   .ToList();

                listDataPeripher.ItemsSource = filteredPeripherals;
            }
        }

        private void LoadWarehouseTypes()
        {
            var types = AppData.db.WarehouseType.ToList();
            FilterWarehouseType.ItemsSource = types;
        }

        // Фильтрация по подтипу склада
        private void FilterSubypeWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterSubypeWarehouse.SelectedValue != null)
            {
                int subtypeId = (int)FilterSubypeWarehouse.SelectedValue;
                var spareParts = AppData.db.SpareParts.Where(sp => sp.IDSubtypeWarehouse == subtypeId).ToList();
                ListDataSpareParts.ItemsSource = spareParts;
            }
        }

        // Фильтрация по типу склада
        private void FilterWarehouseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Сначала очищаем ComboBox подтипов склада
            FilterSubypeWarehouse.ItemsSource = null;
            FilterSubypeWarehouse.SelectedItem = null;

            if (FilterWarehouseType.SelectedValue != null)
            {
                int typeId = (int)FilterWarehouseType.SelectedValue;

                // Загружаем подтипы, соответствующие выбранному типу
                var subtypes = AppData.db.SubtypeWarehouseType.Where(st => st.WarehouseTypeId == typeId).ToList();
                FilterSubypeWarehouse.ItemsSource = subtypes;

                // Фильтруем запчасти по выбранному типу склада
                var spareParts = AppData.db.SpareParts.Where(sp => sp.IDTypeWarehouse == typeId).ToList();
                ListDataSpareParts.ItemsSource = spareParts;
            }
        }

    }
}
