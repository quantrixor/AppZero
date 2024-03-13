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
        public User CurrentUser { get; set; }
        public ViewPageEmp(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            this.DataContext = this;
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
                var documentStart = document.Content.Start;

                // Руководителю ООО «Экострой»
                var headerTo = document.Range(documentStart);
                headerTo.Text = "Руководителю ООО «Экострой»\nОт";
                headerTo.Font.Size = 14;
                headerTo.Font.Bold = 1;
                headerTo.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                headerTo.InsertParagraphAfter();

                // Отчет по складу
                var headerTitle = headerTo.Paragraphs.Add();
                headerTitle.Range.Text = "Отчет по складу";
                headerTitle.Range.Font.Size = 14;
                headerTitle.Range.ParagraphFormat.SpaceBefore = 6;
                headerTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerTitle.Range.InsertParagraphAfter();

                var paragrah = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragrah.Range;
                var listDataSpareParts = SparePartsDestination;
                var table = document.Tables.Add(tableRange, listDataSpareParts.Count + 1, 8);
                table.Range.Font.Size = 10;
                table.Borders.Enable = 1;
                table.Title = "Данные";
                table.Cell(1, 1).Range.Text = "Номер стеллажа";
                table.Cell(1, 2).Range.Text = "Номера полок";
                table.Cell(1, 3).Range.Text = "Описание";
                table.Cell(1, 4).Range.Text = "Тип";
                table.Cell(1, 5).Range.Text = "Подтип";
                table.Cell(1, 6).Range.Text = "Количество";
                table.Cell(1, 7).Range.Text = "Дата";
                table.Cell(1, 8).Range.Text = "Составитель";

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
                    table.Cell(i, 8).Range.Text = $"{CurrentUser.LastName} {CurrentUser.FirstName} {CurrentUser.MiddleName}";
                    i++;
                }
                document.Content.Paragraphs.Last.Range.InsertParagraphAfter();
                document.Content.Paragraphs.Last.Range.ParagraphFormat.SpaceAfter = 6;

                // Переход к концу документа перед добавлением подписей
                Word.Range endOfDoc = document.Content;
                endOfDoc.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

                // Функция для добавления подписи
                void AddSignatureLine(string labelText)
                {
                    Word.Paragraph paragraph = document.Content.Paragraphs.Add();
                    paragraph.Range.Text = labelText;
                    paragraph.Format.LineSpacing = 12;
                    paragraph.Range.Font.Bold = 0;
                    paragraph.Range.Font.Underline = Word.WdUnderline.wdUnderlineNone;
                    paragraph.Range.Font.Size = 14;
                    paragraph.Range.ParagraphFormat.SpaceAfter = 0;
                    paragraph.Range.ParagraphFormat.SpaceBefore = 0;
                    paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    paragraph.Range.InsertParagraphAfter();
                }

                // Добавление строк подписи
                AddSignatureLine("ФИО принявшего: _______________________________");
                AddSignatureLine("Подпись составившего отчет: ______________________");
                AddSignatureLine("Подпись принявшего отчет:   ______________________");

                // Добавление даты
                Word.Paragraph dateParagraph = document.Content.Paragraphs.Add();
                dateParagraph.Range.Text = "Дата: " + DateTime.Now.ToString("dd.MM.yyyy");
                dateParagraph.Range.Font.Bold = 0;
                dateParagraph.Range.Font.Underline = Word.WdUnderline.wdUnderlineNone;
                dateParagraph.Range.ParagraphFormat.SpaceAfter = 0;
                dateParagraph.Range.ParagraphFormat.SpaceBefore = 6;
                dateParagraph.Range.Font.Bold = 1;
                dateParagraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                dateParagraph.Range.InsertParagraphAfter();

                document.SaveAs2($"{Environment.CurrentDirectory}\\Data.pdf", Word.WdSaveFormat.wdFormatPDF);
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

                var documentStart = document.Content.Start;
                // Руководителю ООО «Экострой»
                var headerTo = document.Range(documentStart);
                headerTo.Text = "Руководителю ООО «Экострой»\nОт";
                headerTo.Font.Size = 14;
                headerTo.Font.Bold = 1;
                headerTo.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                headerTo.InsertParagraphAfter();

                // Отчет по складу
                var headerTitle = headerTo.Paragraphs.Add();
                headerTitle.Range.Text = "Отчет по залу";
                headerTitle.Range.Font.Size = 14;
                headerTitle.Range.ParagraphFormat.SpaceBefore = 6;
                headerTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerTitle.Range.InsertParagraphAfter();

                var paragrah = word.ActiveDocument.Paragraphs.Add();
                var tableRange = paragrah.Range;
                var listDataSpareParts = PeripheralsDestination;
                var table = document.Tables.Add(tableRange, listDataSpareParts.Count + 1, 6);
                table.Range.Font.Size = 10;
                table.Borders.Enable = 1;
                table.Title = "Данные";
                table.Cell(1, 1).Range.Text = "Номер стеллажа";
                table.Cell(1, 2).Range.Text = "Номера полок";
                table.Cell(1, 3).Range.Text = "Описание";
                table.Cell(1, 4).Range.Text = "Количество";
                table.Cell(1, 5).Range.Text = "Дата";
                table.Cell(1, 6).Range.Text = "Составитель";

                int i = 2;
                foreach (var item in listDataSpareParts)
                {
                    table.Cell(i, 1).Range.Text = item.Rack.Number;
                    table.Cell(i, 2).Range.Text = item.ShellRackNumberPeripherals;
                    table.Cell(i, 3).Range.Text = item.Description;
                    table.Cell(i, 4).Range.Text = item.Count.ToString();
                    table.Cell(i, 5).Range.Text = item.DateAdded.ToString();
                    table.Cell(i, 6).Range.Text = $"{CurrentUser.LastName} {CurrentUser.FirstName} {CurrentUser.MiddleName}";
                    i++;
                }

                document.Content.Paragraphs.Last.Range.InsertParagraphAfter();
                document.Content.Paragraphs.Last.Range.ParagraphFormat.SpaceAfter = 6;

                // Переход к концу документа перед добавлением подписей
                Word.Range endOfDoc = document.Content;
                endOfDoc.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

                // Функция для добавления подписи
                void AddSignatureLine(string labelText)
                {
                    Word.Paragraph paragraph = document.Content.Paragraphs.Add();
                    paragraph.Range.Text = labelText;
                    paragraph.Format.LineSpacing = 12;
                    paragraph.Range.Font.Bold = 0;
                    paragraph.Range.Font.Underline = Word.WdUnderline.wdUnderlineNone;
                    paragraph.Range.Font.Size = 14;
                    paragraph.Range.ParagraphFormat.SpaceAfter = 0;
                    paragraph.Range.ParagraphFormat.SpaceBefore = 0;
                    paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    paragraph.Range.InsertParagraphAfter();
                }

                // Добавление строк подписи
                AddSignatureLine("ФИО принявшего: _______________________________");
                AddSignatureLine("Подпись составившего отчет: ______________________");
                AddSignatureLine("Подпись принявшего отчет: ______________________");

                // Добавление даты
                Word.Paragraph dateParagraph = document.Content.Paragraphs.Add();
                dateParagraph.Range.Text = "Дата: " + DateTime.Now.ToString("dd.MM.yyyy");
                dateParagraph.Range.Font.Bold = 0;
                dateParagraph.Range.Font.Underline = Word.WdUnderline.wdUnderlineNone;
                dateParagraph.Range.ParagraphFormat.SpaceAfter = 0;
                dateParagraph.Range.ParagraphFormat.SpaceBefore = 6;
                dateParagraph.Range.Font.Bold = 1;
                dateParagraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                dateParagraph.Range.InsertParagraphAfter();

                document.SaveAs2($"{Environment.CurrentDirectory}\\DataPeripher.pdf", Word.WdSaveFormat.wdFormatPDF);
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
        
        // Фильтрация по типу Зала
        private void FilterTypeHallComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Сначала очищаем ComboBox подтипов склада
            FilterSubtypeHallComboBox.ItemsSource = null;
            FilterSubtypeHallComboBox.SelectedItem = null;

            if (FilterTypeHallComboBox.SelectedValue != null)
            {
                int typeId = (int)FilterTypeHallComboBox.SelectedValue;

                // Загружаем подтипы, соответствующие выбранному типу
                var subtypes = AppData.db.SubtypeHall.Where(st => st.IDTypeHall == typeId).ToList();
                FilterSubtypeHallComboBox.ItemsSource = subtypes;

                // Фильтруем запчасти по выбранному типу зала
                var peripherals = AppData.db.Peripherals.Where(sp => sp.IDTypeHall == typeId).ToList();
                listDataPeripher.ItemsSource = peripherals;
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

        private void FilterSubtypeHallComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterSubtypeHallComboBox.SelectedValue != null)
            {
                int subtypeId = (int)FilterSubtypeHallComboBox.SelectedValue;
                var peripherals = AppData.db.Peripherals.Where(sp => sp.IDSubtypeHall == subtypeId).ToList();
                listDataPeripher.ItemsSource = peripherals;
            }
        }
    }
}
