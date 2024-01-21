using System.Data.Entity.Validation;
using System.Text;
using System.Windows;

namespace AppZero.Settings
{
    internal static class CatchException
    {
        internal static void DisplayValidationErrors(DbEntityValidationException ex)
        {
            var errorMessages = new StringBuilder();

            foreach (var validationErrors in ex.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    errorMessages.AppendLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                }
            }

            MessageBox.Show(errorMessages.ToString(), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
