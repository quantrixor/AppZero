using AppZero.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppZero.Model
{
    public partial class SpareParts
    {
        public string ShellRackNumber
        {
            get
            {
                // Получить полки, на которых хранятся запчасти
                var occupiedShelves = AppData.db.SparePartsShelves
                    .Where(sps => sps.IDSpareParts == this.ID) // Фильтрация по текущим запчастям
                    .Select(sps => sps.Shelves)
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
