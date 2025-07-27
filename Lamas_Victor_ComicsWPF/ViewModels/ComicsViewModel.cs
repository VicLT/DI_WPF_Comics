using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services;
using Lamas_Victor_ComicsWPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ViewModels
{
    public class ComicsViewModel : BaseViewModel
    {
        // Fields
        private readonly MainViewModel _mainViewModel;

        private Comic? selectedComic;
        private ObservableCollection<Comic>? comics;
        private ObservableCollection<Comic>? comicsFiltrados;
        private int comicId;
        private int? autorId;
        private string? nombreComic;
        private string? nombreAutor;
        private string? nombreEditorial;
        private decimal? precioVenta;
        private decimal? precioCompra;
        private int? stockLocal;
        private ObservableCollection<StockComic>? stockRestoLocales;
        private string modoOperacion = "Detalles";
        private string busqueda = "Buscar por cómic, autor y/o editorial.";

        // Properties
        public Comic? SelectedComic
        {
            get { return selectedComic; }
            set { selectedComic = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Comic>? Comics
        {
            get { return comics; }
            set { comics = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Comic>? ComicsFiltrados
        {
            get { return comicsFiltrados; }
            set { comicsFiltrados = value; OnPropertyChanged(); }
        }
        public int ComicId
        {
            get { return comicId; }
            set { comicId = value; OnPropertyChanged(); }
        }
        public int? AutorId
        {
            get { return autorId; }
            set { autorId = value; OnPropertyChanged(); }
        }
        public string? NombreComic
        {
            get { return nombreComic; }
            set { nombreComic = value; OnPropertyChanged(); }
        }
        public string? NombreAutor
        {
            get { return nombreAutor; }
            set { nombreAutor = value; OnPropertyChanged(); }
        }
        public string? NombreEditorial
        {
            get { return nombreEditorial; }
            set { nombreEditorial = value; OnPropertyChanged(); }
        }
        public decimal? PrecioVenta
        {
            get { return precioVenta; }
            set { precioVenta = value; OnPropertyChanged(); }
        }
        public decimal? PrecioCompra
        {
            get { return precioCompra; }
            set { precioCompra = value; OnPropertyChanged(); }
        }
        public int? StockLocal
        {
            get { return stockLocal; }
            set { stockLocal = value; OnPropertyChanged(); }
        }
        public string Busqueda
        {
            get { return busqueda; }
            set
            {
                busqueda = value;
                OnPropertyChanged();
                PerformFilterData();
            }
        }
        public ObservableCollection<StockComic>? StockRestoLocales
        {
            get { return stockRestoLocales; }
            set { stockRestoLocales = value; OnPropertyChanged(); }
        }
        public string ModoOperacion
        {
            get { return modoOperacion.ToUpper(); }
            set { modoOperacion = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand LoadCommand { get; }
        public ICommand FilterDataGridCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }
        public ICommand ShowComicsNuevoViewCommand { get; }
        public ICommand ShowComicsEditarViewCommand { get; }
        public ICommand ShowComicsEliminarViewCommand { get; }

        // Constructor
        public ComicsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            LoadCommand = new RelayCommand(PerformCargarComics);
            FilterDataGridCommand = new RelayCommand(PerformFilterData);
            SelectedItemChangedCommand = new RelayCommand(
                PerformSelectedItemChangedCommand);
            ShowComicsNuevoViewCommand = new RelayCommand(
                PerformShowComicsOperacionesViewCommand);
            ShowComicsEditarViewCommand = new RelayCommand(
                PerformShowComicsOperacionesViewCommand,
                CanExecuteComicsOperaciones);
            ShowComicsEliminarViewCommand = new RelayCommand(
                PerformShowComicsOperacionesViewCommand,
                CanExecuteComicsOperaciones);
        }

        /// <summary>
        /// Comprueba si se puede ejecutar el comando de operaciones de cómics.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        /// <returns>True si permite la ejecución.</returns>
        private bool CanExecuteComicsOperaciones(object? parameter = null)
        {
            return SelectedComic != null;
        }

        /// <summary>
        /// Carga la lista de cómics con stock del local del empleado.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformCargarComics(object? parameter = null)
        {
            using (var cs = new ComicsService())
            {
                Comics = cs.CargarComicsPorLocalObservables();
                ComicsFiltrados = new ObservableCollection<Comic>(Comics);
            }
        }

        /// <summary>
        /// Filtra los cómics según el texto de búsqueda.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformFilterData(object? parameter = null)
        {
            if (Comics != null)
            {
                if (string.IsNullOrWhiteSpace(Busqueda) ||
                    Busqueda == "Buscar por cómic, autor y/o editorial.")
                {
                    ComicsFiltrados = new ObservableCollection<Comic>(Comics);
                }
                else
                {
                    var filtro = Busqueda.ToLower();
                    ComicsFiltrados = new ObservableCollection<Comic>(
                        Comics.Where(c =>
                            (c.Nombre?.ToLower().Contains(filtro) ?? false) ||
                            (c.Autor?.NombreCompleto?.ToLower().Contains(filtro) ?? false) ||
                            (c.Editorial?.Nombre?.ToLower().Contains(filtro) ?? false)
                        ).ToList());
                }
            }
        }

        /// <summary>
        /// Carga los datos del cómic seleccionado en los campos de la vista.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformSelectedItemChangedCommand(object? parameter = null)
        {
            if (SelectedComic != null)
            {
                NombreComic = SelectedComic.Nombre;
                NombreAutor = SelectedComic.Autor?.Nombre + " " +
                    SelectedComic.Autor?.Apellido;
                NombreEditorial = SelectedComic.Editorial?.Nombre;
                PrecioVenta = SelectedComic.PrecioVenta;
                AutorId = SelectedComic.Autor?.AutorId;
                ComicId = SelectedComic.ComicId;
                PrecioCompra = SelectedComic.PrecioCompra;
                using (var scs = new StockComicsService())
                {
                    StockLocal = scs.StockComicTienda(SelectedComic.ComicId);
                    StockRestoLocales = scs.StockComicRestoLocales(SelectedComic.ComicId);
                }
            }
        }

        /// <summary>
        /// Muestra la vista correspondiente.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformShowComicsOperacionesViewCommand(object? parameter = null)
        {
            if (parameter != null)
            {
                ModoOperacion = (string)parameter;
            }

            ComicsOperacionesViewModel.ModoOperacion modoOperacionEnum = parameter switch
            {
                "NUEVO" => ComicsOperacionesViewModel.ModoOperacion.NUEVO,
                "EDITAR" => ComicsOperacionesViewModel.ModoOperacion.EDITAR,
                "ELIMINAR" => ComicsOperacionesViewModel.ModoOperacion.ELIMINAR,
                _ => throw new ArgumentException("Modo de operación inválido", nameof(modoOperacion))
            };

            _mainViewModel.CurrentChildView = new ComicsOperacionesViewModel(
                _mainViewModel, SelectedComic, StockLocal ?? 0, modoOperacionEnum);
        }
    }
}
