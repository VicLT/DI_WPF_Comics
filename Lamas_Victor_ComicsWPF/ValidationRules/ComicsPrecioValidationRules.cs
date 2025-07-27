using System.Globalization;
using System.Windows.Controls;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ValidationRules
{
    class ComicsPrecioValidationRules : ValidationRule
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        /// <summary>
        /// Valida que el valor introducido sea un decimal en el rango Min-Max.
        /// </summary>
        /// <param name="value">Valor que recibe de la vista.</param>
        /// <param name="cultureInfo">
        /// Propiedades de formato locales numéricas, de fecha, etc.
        /// </param>
        /// <returns>Objeto que indica si la validación fue exitosa.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal precio = 0;

            try
            {
                if (((string)value).Length > 0)
                {
                    precio = decimal.Parse((String)value);
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(
                    false,
                    "Introduzca un valor (decimal) en rango: " + Min + " - " + Max
                );
            }

            if ((precio < Min) || (precio > Max))
            {
                return new ValidationResult(
                    false,
                    "Introduzca un valor (decimal) en rango: " + Min + " - " + Max
                );
            }

            return ValidationResult.ValidResult;
        }
    }
}
