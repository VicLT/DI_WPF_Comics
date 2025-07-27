using System.Globalization;
using System.Windows.Controls;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ValidationRules
{
    class ComicsCantidadValidationRules : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        /// <summary>
        /// Valida que el valor introducido sea un entero en el rango Min-Max.
        /// </summary>
        /// <param name="value">Valor que recibe de la vista.</param>
        /// <param name="cultureInfo">
        /// Propiedades de formato locales numéricas, de fecha, etc.
        /// </param>
        /// <returns>Objeto que indica si la validación fue exitosa.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int cantidad = 0;

            try
            {
                if (((string)value).Length > 0)
                {
                    cantidad = int.Parse((String)value);
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(
                    false,
                    "Introduzca un valor entero en rango: " + Min + " - " + Max
                );
            }

            if ((cantidad < Min) || (cantidad > Max))
            {
                return new ValidationResult(
                    false,
                    "Introduzca un valor entero en rango: " + Min + " - " + Max
                );
            }
            
            return ValidationResult.ValidResult;
        }
    }
}
