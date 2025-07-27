using System.Windows.Media.Imaging;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models
{
    /// <summary>Singleton para el empleado que ha hecho login.</summary>
    public class EmpleadoLogged
    {
        private static EmpleadoLogged? _instance;
        private static readonly object _lock = new object();
        private EmpleadoLogged() { }

        public static EmpleadoLogged Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new EmpleadoLogged();
                    }
                    return _instance;
                }
            }
        }

        public Empleado? empleado { get; set; } = null;
        public BitmapImage? imagenBitMap { get; set; } =
            new BitmapImage(new Uri("pack://application:,,,/Resources/user-icon.png"));
        public Local? localEmpleado { get; set; } = null;
    }
}