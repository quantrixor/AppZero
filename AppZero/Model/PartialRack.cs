using AppZero.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppZero.Model
{
    public partial class Rack
    {
        public string ShellRackNumber
        {
            get
            {
                // Получить занятые полки для текущей периферии
                var occupiedShelves = AppData.db.PeripheralShelf
                    .Where(ps => ps.Peripherals.ID == this.ID) // Фильтрация по текущей периферии
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
