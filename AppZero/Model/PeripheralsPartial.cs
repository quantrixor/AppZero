using AppZero.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppZero.Model
{
    public partial class Peripherals
    {

        public string ShellRackNumber
        {
            get
            {
                // Получить занятые полки для текущей периферии
                var occupiedShelves = AppData.db.PeripheralShelf
                    .Where(ps => ps.PeripheralID == this.ID) // Фильтрация по текущей периферии
                    .Select(ps => ps.Shelves)
                    .OrderBy(s => s.IDRack) // Сортировка по номеру стеллажа
                    .ThenBy(s => s.Number) // Затем сортировка по номеру полки
                    .ToList();

                StringBuilder occupiedShelvesNumbers = new StringBuilder();

                // Вывести номера занятых полок
                int currentRackNumber = -1;
                foreach (var shelf in occupiedShelves)
                {
                    if (shelf.IDRack != currentRackNumber)
                    {
                        if (currentRackNumber != -1)
                        {
                            occupiedShelvesNumbers.Length -= 2; // Удалить лишнюю запятую и пробел перед переходом к следующему стеллажу
                            occupiedShelvesNumbers.Append(" | ");
                        }
                        currentRackNumber = shelf.IDRack;
                    }

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
