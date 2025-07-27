using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.ViewModels.Base;
using System.Windows.Input;
using System.Windows.Media.Imaging;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ViewModels
{
    /// <summary>
    /// ViewModel para la vista principal.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        // Fields
        private Empleado? empleado = EmpleadoLogged.Instance?.empleado;
        private BitmapImage? imagen = EmpleadoLogged.Instance?.imagenBitMap;
        private Local? local = EmpleadoLogged.Instance?.localEmpleado;
        private BaseViewModel? currentChildView;

        // Properties
        public Empleado? Empleado
        {
            get { return empleado; }
            set { empleado = value; OnPropertyChanged(); }
        }
        public BitmapImage? Imagen
        {
            get { return imagen; }
            set { imagen = value; OnPropertyChanged(); }
        }
        public Local? Local
        {
            get { return local; }
            set { local = value; OnPropertyChanged(); }
        }
        public BaseViewModel? CurrentChildView
        {
            get { return currentChildView; }
            set { currentChildView = value; OnPropertyChanged(); }
        }
        public string NombreCompleto
        {
            get
            {
                return Empleado?.Nombre?.ToUpper() + " " + Empleado?.Apellido?.ToUpper();
            }
        }

        // Commands
        public ICommand ShowInicioViewCommand { get; }
        public ICommand ShowComicsViewCommand { get; }
        public ICommand ShowOperacionesViewCommand { get; }
        public ICommand ShowStockViewCommand { get; }        
        public ICommand ShowClientesViewCommand { get; }
        public ICommand ShowEstadisticasViewCommand { get; }

        // Constructor
        public MainViewModel()
        {
            // Default view
            PerformShowInicioViewCommand(null);

            // Initialize commands
            ShowInicioViewCommand = new RelayCommand(PerformShowInicioViewCommand);
            ShowComicsViewCommand = new RelayCommand(PerformShowComicsViewCommand);
            ShowOperacionesViewCommand = new RelayCommand(PerformShowOperacionesViewCommand);
            ShowStockViewCommand = new RelayCommand(PerformShowStockViewCommand);
            ShowClientesViewCommand = new RelayCommand(PerformShowClientesViewCommand);
            ShowEstadisticasViewCommand = new RelayCommand(PerformShowEstadisticasViewCommand);
        }

        /// <summary>
        /// Muestra la vista correspondiente.
        /// </summary>
        /// <param name="parameter">(object) Parámetro del comando.</param>
        public void PerformShowInicioViewCommand(object? parameter = null)
        {
            CurrentChildView = new InicioViewModel();
        }
        public void PerformShowComicsViewCommand(object? parameter = null)
        {
            CurrentChildView = new ComicsViewModel(this);
        }
        public void PerformShowOperacionesViewCommand(object? parameter = null)
        {
            CurrentChildView = new OperacionesViewModel();
        }
        public void PerformShowStockViewCommand(object? parameter = null)
        {
            CurrentChildView = new StockViewModel();
        }
        public void PerformShowClientesViewCommand(object? parameter = null)
        {
            CurrentChildView = new ClientesViewModel();
        }
        public void PerformShowEstadisticasViewCommand(object? parameter = null)
        {
            CurrentChildView = new EstadisticasViewModel();
        }
    }
}
