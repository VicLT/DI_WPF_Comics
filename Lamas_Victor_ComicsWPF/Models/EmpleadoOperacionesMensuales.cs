/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models
{
    public class EmpleadoOperacionesMensuales
    {
        public string nombreCompleto { get; set; }
        public string iniciales { get; set; }
        public int[] cantidadOperacionesMensuales { get; set; } // (0=Enero, 1=Febrero...)

        public EmpleadoOperacionesMensuales(string nombreCompleto)
        {
            this.nombreCompleto = nombreCompleto;
            iniciales = string.Empty;
            cantidadOperacionesMensuales = new int[12];
        }
    }
}
