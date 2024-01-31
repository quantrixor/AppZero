using AppZero.Context;
using AppZero.Model;
using AppZero.Settings;
using AppZero.Views.Windows.AdminWindows;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;

namespace AppZero.Views.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ViewPage.xaml
    /// </summary>
    public partial class ViewPage : Page
    {
        public List<SpareParts> SparePartsDestination = new List<SpareParts>();
        public List<Peripherals> PeripheralsDestination = new List<Peripherals>();
        public User User { get; set; }
        public SignIn SignIn { get; set; }
        public List<Position> Positions { get; set; }
        public List<Rule> Rules { get; set; }
        public User CurrentUser { get; set; }
        private User _selectedItem { get; set; }

        public ViewPage(User currentUser)
        {
            InitializeComponent();
            User = new User();
            SignIn = new SignIn();
            Positions = AppData.db.Position.ToList();
            Rules = AppData.db.Rule.ToList();
            CurrentUser = currentUser;

            this.DataContext = this;
            MessageBox.Show($"Добро пожаловать {currentUser.FirstName} {CurrentUser.LastName}! Вы авторизовались правами Администратора.",
               "Систеиное уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Добавляем сотрудника в БД
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbFirstName.Text == "" ||
                     txbLastName.Text == "" ||
                     txbMiddleName.Text == "" ||
                     txbPassword.Text == "" ||
                     txbUsername.Text == "" ||
                     cmbPosition.Text == "" ||
                     cmbRule.Text == "")
                    throw new Exception("Заполните поля!");

                if (User.ID == 0 && SignIn.ID == 0 && _selectedItem == null)
                {
                    if (AppData.db.SignIn.Count(item => item.Username == SignIn.Username) > 0)
                        throw new Exception("Пользователь: " + SignIn.Username + " уже существует!");

                    SignIn.IDRole = AppData.db.Rule.FirstOrDefault(item => item.Title == cmbRule.Text).ID;
                    SignIn = new SignIn
                    {
                        Username = txbUsername.Text,
                        Password = txbPassword.Text,
                        Rule = (Rule)cmbRule.SelectedItem
                    };
                    AppData.db.SignIn.Add(SignIn);

                    User = new User
                    {
                        FirstName = txbFirstName.Text,
                        LastName = txbLastName.Text,
                        MiddleName = txbMiddleName.Text,
                        Position = (Position)cmbPosition.SelectedItem
                    };

                    AppData.db.User.Add(User);
                }
                if (_selectedItem != null)
                {
                    _selectedItem.FirstName = txbFirstName.Text;
                    _selectedItem.LastName = txbLastName.Text;
                    _selectedItem.MiddleName = txbMiddleName.Text;
                    _selectedItem.SignIn.Username = txbUsername.Text;
                    _selectedItem.SignIn.Password = txbPassword.Text;
                    _selectedItem.Position = cmbPosition.SelectedItem as Position;
                    _selectedItem.SignIn.Rule = cmbRule.SelectedItem as Rule;
                }

                AppData.db.SaveChanges();

                MessageBox.Show("Данные успешно сохранены в базу данных!", "Данные сохранены.", MessageBoxButton.OK, MessageBoxImage.Information);
                GC.Collect();
                ClearUserData();
                Page_Loaded(null, null);
                mainTabControl.SelectedIndex = 1;
            }
            catch (DbEntityValidationException ex)
            {
                CatchException.DisplayValidationErrors(ex);
            }
            finally
            {
                _selectedItem = null;
            }
        }
        // Очистка всех значений из полей
        private void ClearUserData()
        {
            txbFirstName.Text = "";
            txbLastName.Text = "";
            txbMiddleName.Text = "";
            txbPassword.Text = "";
            txbUsername.Text = "";
            cmbPosition.Text = "";
            cmbRule.Text = "";
            _selectedItem = null;
            User = null;
        }

        // Выгружаем данные из БД
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListDataEmp.ItemsSource = AppData.db.User.ToList();

            ListDataSpareParts.ItemsSource = AppData.db.SpareParts.ToList();
            listDataPeripher.ItemsSource = AppData.db.Peripherals.ToList();
            FilterTypeHallComboBox.ItemsSource = AppData.db.TypeHall.ToList();
            LoadWarehouseTypes();
        }

        // Поиск сотрудников
        private void txbSearchEmp_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Поиск по следующим критериям: ID, Имя, Фамилия и Имя пользователя
            ListDataEmp.ItemsSource = AppData.db.User.Where(item => item.FirstName.Contains(txbSearchEmp.Text) ||
            item.LastName.Contains(txbSearchEmp.Text) ||
            item.SignIn.Username.Contains(txbSearchEmp.Text) ||
            item.ID.ToString().Contains(txbSearchEmp.Text)).ToList();
        }

        // Поиск по складу
        private void txbSearchDevice_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Поиск по следующим критериям: ID, Номер стеллажа, Номер полки и Количество на складе
                ListDataSpareParts.ItemsSource = AppData.db.SpareParts.Where(item => item.ID.ToString().Contains(txbSearchDevice.Text) ||
                        item.Rack.Number.Contains(txbSearchDevice.Text) || item.WarehouseType.Title.Contains(txbSearchDevice.Text) ||
                        item.Description.Contains(txbSearchDevice.Text) ||
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

        // Поиск по залу
        private void txbSearchPeripher_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Поиск по следующим критериям: ID, Номер стеллажа, Номер полки и Количество на складе
                listDataPeripher.ItemsSource = AppData.db.Peripherals.Where(item => item.ID.ToString().Contains(txbSearchPeripher.Text) ||
                item.Rack.Number.Contains(txbSearchPeripher.Text) ||
                item.TypeHall.Titiel.Contains(txbSearchPeripher.Text) ||
                item.Description.Contains(txbSearchPeripher.Text) ||
                item.Count.ToString().Contains(txbSearchPeripher.Text)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        // Сортировка данных зала по дате
        private void sortDatePeripher_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            listDataPeripher.ItemsSource = AppData.db.Peripherals.Where(item => item.DateAdded == sortDatePeripher.SelectedDate).ToList();
        }
        // Обновить данные из базы данных
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            sortDate.SelectedDate = null;
            sortDatePeripher.SelectedDate = null;
            FilterTypeHallComboBox.SelectedItem = null;
            FilterWarehouseType.SelectedItem = null;
            FilterSubypeWarehouse.SelectedItem = null;
            Page_Loaded(null, null);
        }

        // Печать данных склада
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpStartDate.SelectedDate != null && dtpEndDate.SelectedDate != null)
                {
                    // Запрос на вывод данных склада по указанным параметрам
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

        private void ExportSparePartsDataPDF()
        {
            var word = new Word.Application();
            try
            {
                var document = word.Documents.Add();
                // Название организации
                var orgName = "Название организации";
                var orgNameParagraph = document.Paragraphs.Add();
                orgNameParagraph.Range.Text = orgName;
                orgNameParagraph.Range.Font.Size = 18;
                orgNameParagraph.Range.Font.Bold = 1;
                orgNameParagraph.Range.InsertParagraphAfter();
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
                // Дата формирования отчета
                var reportDate = DateTime.Now.ToString("dd.MM.yyyy");
                var reportDateParagraph = document.Paragraphs.Add();
                reportDateParagraph.Range.Text = $"Дата формирования отчета: {reportDate}";
                reportDateParagraph.Range.InsertParagraphBefore();

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
                // Название организации
                var orgName = "Название организации";
                var orgNameParagraph = document.Paragraphs.Add();
                orgNameParagraph.Range.Text = orgName;
                orgNameParagraph.Range.Font.Size = 18;
                orgNameParagraph.Range.Font.Bold = 1;
                orgNameParagraph.Range.InsertParagraphAfter();
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
                // Дата формирования отчета
                var reportDate = DateTime.Now.ToString("dd.MM.yyyy");
                var reportDateParagraph = document.Paragraphs.Add();
                reportDateParagraph.Range.Text = $"Дата формирования отчета: {reportDate}";
                reportDateParagraph.Range.InsertParagraphBefore();
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
        // Печать данных зала
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
        // Переход в окно добавления данных склада
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ActionSparePartsWindow actionSparePartsWindow = new ActionSparePartsWindow(new SpareParts());
            actionSparePartsWindow.ShowDialog();
        }
        // Переход в окно редактирования данных склада
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedSpareParts = (SpareParts)ListDataSpareParts.SelectedItem;
            if (selectedSpareParts != null)
            {
                ActionSparePartsWindow actionSparePartsWindow = new ActionSparePartsWindow(selectedSpareParts);
                actionSparePartsWindow.ShowDialog();
            }
        }
        // Удаление данных склада
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedSpareParts = (SpareParts)ListDataSpareParts.SelectedItem;
                if (selectedSpareParts != null)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить выбранный объект из Базы данных?", "Внимание! Данные удалятся навсегда.",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var shelvesSpare = AppData.db.SparePartsShelves.FirstOrDefault(item => item.IDSpareParts == selectedSpareParts.ID);
                        if (shelvesSpare != null)
                        {
                            AppData.db.SparePartsShelves.Remove(shelvesSpare);
                        }
                        AppData.db.SpareParts.Remove(selectedSpareParts);
                        AppData.db.SaveChanges();
                        Page_Loaded(null, null);
                        MessageBox.Show("Данные успешно удалились!", "Операция выполнена", MessageBoxButton.OK, MessageBoxImage.Information);
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
        // Переход в окно добавления данных зала
        private void btnAddPeripherals_Click(object sender, RoutedEventArgs e)
        {
            ActionPeripheralsWindow actionPeripheralsWindow = new ActionPeripheralsWindow(new Peripherals());
            actionPeripheralsWindow.ShowDialog();
        }
        // Переход в окно редактирования данных зала
        private void btnEditPeripherals_Click(object sender, RoutedEventArgs e)
        {
            var selectedPeripherals = (Peripherals)listDataPeripher.SelectedItem;
            if (selectedPeripherals != null)
            {
                ActionPeripheralsWindow actionPeripheralsWindow = new ActionPeripheralsWindow(selectedPeripherals);
                actionPeripheralsWindow.ShowDialog();
            }
        }
        // Удаление данных зала
        private void btnRemovePeripherals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPeripherals = (Peripherals)listDataPeripher.SelectedItem;
                if (selectedPeripherals != null)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить выбранный объект из Базы данных?", "Внимание! Данные удалятся навсегда.",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        // Получение всех записей PeripheralShelf, связанных с выбранным периферийным устройством
                        var selectedPeripheralShelves = AppData.db.PeripheralShelf.Where(item => item.PeripheralID == selectedPeripherals.ID).ToList();

                        // Удаление всех найденных записей PeripheralShelf
                        foreach (var selectedPerShelf in selectedPeripheralShelves)
                        {
                            AppData.db.PeripheralShelf.Remove(selectedPerShelf);
                        }

                        AppData.db.Peripherals.Remove(selectedPeripherals);
                        AppData.db.SaveChanges();
                        Page_Loaded(null, null);
                        MessageBox.Show("Данные успешно удалились!", "Операция выполнена", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public Rack Rack { get; set; }
        public Shelves Shelves { get; set; }
        private void btnSaveRack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rack rack = new Rack();
                string rackNumber = txbNumSt.Text;
                int count = int.Parse(txbCountSt.Text);
                if (AppData.db.Rack.Count(item => item.Number == rackNumber) > 0)
                    throw new Exception($"Номер стеллажа {rackNumber} уже существует");
                rack.Number = rackNumber;

                AppData.db.Rack.Add(rack); // Добавляем стеллаж в базу данных
                AppData.db.SaveChanges(); // Сохраняем изменения

                List<Shelves> shelvesList = new List<Shelves>();
                for (int i = 1; i <= count; i++)
                {
                    string num = rackNumber + "." + i.ToString();
                    Shelves shelf = new Shelves { Number = num, IDRack = rack.ID }; // Присваиваем IDRack значение ID только что добавленного стеллажа
                    shelvesList.Add(shelf);
                }

                // Добавляем полки в базу данных
                foreach (var shelf in shelvesList)
                {
                    AppData.db.Shelves.Add(shelf);
                }

                AppData.db.SaveChanges();
                MessageBox.Show("Стеллаж добавлен в БД", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                // Отключаем обработчик событий перед программной очисткой поля
                txbCountSt.TextChanged -= txbCountSt_TextChanged;

                txbNumSt.Text = "";
                txbCountSt.Text = "";

                // Включаем обработчик событий обратно после изменения текста
                txbCountSt.TextChanged += txbCountSt_TextChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void txbCountSt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txbCountSt.Text, out int count))
            {
                if (count < 3)
                {
                    txbCountSt.Text = "3";
                    MessageBox.Show("Введите число от 3 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (count > 5)
                {
                    txbCountSt.Text = "5";
                    MessageBox.Show("Введите число от 3 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txbCountSt.Text = "3";
                MessageBox.Show("Введите число от 3 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItemEmp = (User)ListDataEmp.SelectedItem;
                if (selectedItemEmp != null)
                {
                    if (selectedItemEmp.ID == CurrentUser.ID)
                        throw new Exception("Вы не можете удалить свои данные!");
                    if (MessageBox.Show("Вы действительно хотите удалить выбранный объект из Базы данных?", "Внимание! Данные удалятся навсегда.",
                       MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var selectedSignInDate = AppData.db.SignIn.FirstOrDefault(item => item.ID == selectedItemEmp.IDSignIn);
                        AppData.db.User.Remove(selectedItemEmp);
                        AppData.db.SignIn.Remove(selectedSignInDate);
                        AppData.db.SaveChanges();
                        Page_Loaded(null, null);
                        MessageBox.Show("Данные успешно удалились!", "Операция выполнена", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    throw new Exception("Выберите запись, чтобы удалить!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListDataEmp_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var userData = (User)ListDataEmp.SelectedItem;
                if (userData != null)
                {
                    MessageBox.Show($"Имя пользователя: {userData.SignIn.Username} Пароль: {userData.SignIn.Username} Роль: {userData.SignIn.Rule.Title}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnManageTypeWarehouse_Click(object sender, RoutedEventArgs e)
        {
            ActionTypeWarehouse window = new ActionTypeWarehouse();
            window.ShowDialog();
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

        private void btnManageHallType_Click(object sender, RoutedEventArgs e)
        {
            ActionTypeHallWindow actionTypeHallWindow = new ActionTypeHallWindow();
            actionTypeHallWindow.ShowDialog();
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

        private void FillTextBoxUser(User user)
        {
            txbLastName.Text = user.LastName;
            txbFirstName.Text = user.FirstName;
            txbMiddleName.Text = user.MiddleName;
            cmbPosition.SelectedItem = user.Position;
            cmbRule.SelectedItem = user.SignIn.Rule;
            txbUsername.Text = user.SignIn.Username;
            txbPassword.Text = user.SignIn.Password;
            mainTabControl.SelectedIndex = 0;
        }

        private void ButtonEditUser_Click(object sender, RoutedEventArgs e)
        {
            _selectedItem = (User)ListDataEmp.SelectedItem;
            if(_selectedItem != null)
            {
                FillTextBoxUser(_selectedItem);
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
