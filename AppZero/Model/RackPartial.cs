using AppZero.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppZero.Model
{
    public partial class Rack: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _peripheralId;

        public int PeripheralId
        {
            get => _peripheralId;
            set
            {
                if (_peripheralId != value)
                {
                    _peripheralId = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShellRackNumber));
                }
            }
        }
        private int _selectedPeripheralId;

        public int SelectedPeripheralId
        {
            get => _selectedPeripheralId;
            set
            {
                if (_selectedPeripheralId != value)
                {
                    _selectedPeripheralId = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShellRackNumber));
                }
            }
        }

        public string ShellRackNumber
        {
            get
            {
                // Получить занятые полки для текущего стеллажа
                var occupiedShelves = AppData.db.PeripheralShelf
                    .Where(ps => ps.Shelves.Rack.ID == this.ID) // Фильтрация по текущему стеллажу
                    .Select(ps => ps.Shelves)
                    .OrderBy(s => s.ID)
                    .ToList();

                StringBuilder occupiedShelvesNumbers = new StringBuilder();

                // Вывести номера занятых полок
                foreach (var shelf in occupiedShelves)
                {
                    occupiedShelvesNumbers.Append(shelf.Number + ", ");
                }

                // Удалить лишнюю запятую и пробел в конце строки
                if (occupiedShelves.Count > 0)
                {
                    occupiedShelvesNumbers.Length -= 2;
                }

                return occupiedShelvesNumbers.ToString();
            }
        }


    }
}
