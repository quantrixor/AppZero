using AppZero.Context;
using AppZero.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppZero.Views.Windows.AdminWindows
{
    /// <summary>
    /// Interaction logic for ActionTypeWarehouse.xaml
    /// </summary>
    public partial class ActionTypeWarehouse : Window
    {
        private WarehouseType _selectedType { get; set; }
        private SubtypeWarehouseType _selectedSubType { get; set; }

        public ObservableCollection<WarehouseType> listWarehouseTypes { get; set; }
        public ObservableCollection<SubtypeWarehouseType> listSubtypeWarehouseTypes { get; set; }
        public ActionTypeWarehouse()
        {
            InitializeComponent();

            listWarehouseTypes = new ObservableCollection<WarehouseType>();
            listSubtypeWarehouseTypes = new ObservableCollection<SubtypeWarehouseType>();
            LoadTypes();
            LoadSubtypes();
            this.DataContext = this;
        }
        private void LoadTypes()
        {
            listWarehouseTypes.Clear();

            var typeWarehouse = AppData.db.WarehouseType.ToList();

            foreach (var type in typeWarehouse)
            {
                listWarehouseTypes.Add(type);
            }
        }
        private void LoadSubtypes()
        {
            listSubtypeWarehouseTypes.Clear(); // Очистка текущей коллекции

            if (_selectedType == null)
            {
                // Возможно, стоит очистить список подтипов, если нет выбранного типа.
                listSubtypeWarehouseTypes.Clear();
                return;
            }
            var subtypes = AppData.db.SubtypeWarehouseType
                                     .Where(subtype => subtype.WarehouseTypeId == _selectedType.ID)
                                     .ToList();

            foreach (var subtype in subtypes)
            {
                listSubtypeWarehouseTypes.Add(subtype); // Добавление подтипов в ObservableCollection
            }
        }

        private void ButtonSaveType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(txbTypeName.Text))
                {
                    MessageBox.Show("Пожалуйста заполните поле названия типа!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (_selectedType == null)
                {
                    WarehouseType warehouseType = new WarehouseType()
                    {
                        Title = txbTypeName.Text
                    };
                    AppData.db.WarehouseType.Add(warehouseType);
                }
                else
                {
                    _selectedType.Title = txbTypeName.Text;
                }
                AppData.db.SaveChanges();
                MessageBox.Show("Данные типа сохранены!", "Операция прошла успешно.", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadTypes();
                _selectedType = null;
                txbTypeName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSubtypeSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txbSubTypeName.Text))
                {
                    MessageBox.Show("Пожалуйста заполните поле названия подтипа!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (_selectedType == null)
                {
                    MessageBox.Show("Выберите тип для подтипа!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_selectedSubType == null)
                {
                    SubtypeWarehouseType subtypeWarehouseType = new SubtypeWarehouseType()
                    {
                        Title = txbSubTypeName.Text,
                        WarehouseTypeId = _selectedType.ID // Это предполагаемое название свойства внешнего ключа.
                    };

                    AppData.db.SubtypeWarehouseType.Add(subtypeWarehouseType);
                    AppData.db.SaveChanges(); // Сохраните изменения сразу после добавления

                    listSubtypeWarehouseTypes.Add(subtypeWarehouseType); // Добавьте подтип в ObservableCollection
                }
                else
                {
                    _selectedSubType.Title = txbSubTypeName.Text;
                    AppData.db.SaveChanges(); // Сохраните изменения после редактирования
                }

                // Обновите UI
                LoadSubtypes();
                AppData.db.SaveChanges();
                MessageBox.Show("Данные под типа сохранены!", "Операция прошла успешно.", MessageBoxButton.OK, MessageBoxImage.Information);
                _selectedSubType = null;
                _selectedType = null;
                txbSubTypeName.Text = "";
                LoadSubtypes();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listTypes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _selectedType = (WarehouseType)listTypes.SelectedItem;
            if (_selectedType != null) // Добавьте эту проверку, чтобы избежать ошибок.
            {
                lblSelectedType.Content = $"{_selectedType.ID} - {_selectedType.Title}";
                LoadSubtypes();
            }
        }

        private void ButtonEdittype_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedType = (WarehouseType)listTypes.SelectedItem;
                if (_selectedType != null)
                {
                    txbTypeName.Text = _selectedType.Title;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRemovetype_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedType = (WarehouseType)listTypes.SelectedItem;
                if(_selectedType != null)
                {
                    AppData.db.WarehouseType.Remove(_selectedType);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные типа были удалены!", "Операция прошла успешно.", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTypes();
                    LoadSubtypes();
                    _selectedType = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonEditSubtype_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedSubType = (SubtypeWarehouseType)listSubtypes.SelectedItem;
                if (_selectedSubType != null)
                {
                    txbSubTypeName.Text = _selectedSubType.Title;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonTypeCancel_Click(object sender, RoutedEventArgs e)
        {
            _selectedType = null;
            LoadTypes();
        }

        private void ButtonSubTypeCancel_Click(object sender, RoutedEventArgs e)
        {
            _selectedType = null;
            LoadSubtypes();
        }

        private void ButtonRemoveSubtype_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedSubType = (SubtypeWarehouseType)listSubtypes.SelectedItem;
                if(_selectedSubType != null)
                {
                    AppData.db.SubtypeWarehouseType.Remove(_selectedSubType);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные подтипа были удалены!", "Операция прошла успешно.", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadSubtypes();
                    _selectedType = null;
                    _selectedSubType = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
