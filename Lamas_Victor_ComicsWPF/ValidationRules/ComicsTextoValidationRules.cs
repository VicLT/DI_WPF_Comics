using System.Globalization;
using System.Windows.Controls;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ValidationRules
{
    class ComicsTextoValidationRules : ValidationRule
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        /// <summary>
        /// Validación de texto con longitud mínima y máxima.
        /// </summary>
        /// <param name="value">Valor que recibe de la vista.</param>
        /// <param name="cultureInfo">
        /// Propiedades de formato locales numéricas, de fecha, etc.
        /// </param>
        /// <returns>Objeto que indica si la validación fue exitosa.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string texto = (string)value;

                if (texto.Length <= 0)
                {
                    return new ValidationResult(false, "Este campo es obligatorio.");
                }

                if (texto.Length < MinLength || texto.Length > MaxLength)

                {
                    return new ValidationResult(
                        false,
                        "La longitud debe de estar comprendida entre "
                        + MinLength + " y " + MaxLength + "."
                    );
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }
    }
}
