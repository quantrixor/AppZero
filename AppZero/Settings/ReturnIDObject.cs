using AppZero.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppZero.Settings
{
    public static class ReturnIDObject
    {

        public static int? ReturnRackID(ComboBox comboBox)
        {
			try
			{
                var selectedRack = comboBox.SelectedItem as Rack;
                if (selectedRack != null)
                {
                    return selectedRack.ID;
                }
                else
                {
                    throw new Exception("Стойка не выбрана.");
                }
            }
            catch (Exception ex)
			{
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
			}
        }

        public static int? ReturnWarehouseType(ComboBox comboBox)
        {
            try
            {
                var selectedType = comboBox.SelectedItem as WarehouseType;
                if (selectedType != null)
                {
                    return selectedType.ID;
                }
                else
                {
                    throw new Exception("Тип не выбрана.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static int? ReturnSubWarehouseType(ComboBox comboBox)
        {
            try
            {
                var selectedSubType = comboBox.SelectedItem as SubtypeWarehouseType;
                if (selectedSubType != null)
                {
                    return selectedSubType.ID;
                }
                else
                {
                    throw new Exception("Подтип не выбрана.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
