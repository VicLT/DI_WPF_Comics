using System.Globalization;
using System.Windows.Controls;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ValidationRules
{
    class ComboBoxValidationRules : ValidationRule
    {
        /// <summary>
        /// Valida que el valor del ComboBox no sea nulo o vacío.
        /// </summary>
        /// <param name="value">Valor que recibe de la vista.</param>
        /// <param name="cultureInfo">
        /// Propiedades de formato locales numéricas, de fecha, etc.
        /// </param>
        /// <returns>Objeto que indica si la validación fue exitosa.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "Debe seleccionar un valor.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
