using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services;
using Lamas_Victor_ComicsWPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ViewModels
{
    public class ComicsOperacionesViewModel : BaseViewModel
    {
        // Fields
        public enum ModoOperacion
        {
            NUEVO,
            EDITAR,
            ELIMINAR
        }
        
        private readonly MainViewModel _mainViewModel;

        private Comic? comic;
        private ObservableCollection<Autor>? autores;
        private ObservableCollection<Editorial>? editoriales;
        private int? stockLocal;
        private ModoOperacion modoOp;

        // Properties
        public Comic? Comic
        {
            get { return comic; }
            set { comic = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Autor>? Autores
        {
            get { return autores; }
            set { autores = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Editorial>? Editoriales
        {
            get { return editoriales; }
            set { editoriales = value; OnPropertyChanged(); }
        }
        public int? StockLocal
        {
            get { return stockLocal; }
            set { stockLocal = value; OnPropertyChanged(); }
        }
        public ModoOperacion ModoOp
        {
            get { return modoOp; }
            set { modoOp = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand ShowComicsViewCommand { get; }
        public ICommand ExecuteOperationCommand { get; }

        // Constructor
        public ComicsOperacionesViewModel(MainViewModel mainViewModel,
            Comic? selectedComic, int stockLocal, ModoOperacion? modoOp)
        {
            _mainViewModel = mainViewModel;
            ModoOp = modoOp ?? ModoOperacion.NUEVO;

            using (var autoresService = new AutoresService())
            {
                Autores = autoresService.ListadoAutores();
            }

            using (var editorialesService = new EditorialesService())
            {
                Editoriales = editorialesService.ListarEditorialesObservable();
            }

            if (ModoOp == ModoOperacion.NUEVO)
            {
                Comic = new Comic();
                StockLocal = null;
            }
            else
            {
                Comic = selectedComic;
                StockLocal = stockLocal;
                IgualarReferenciasListas();
            }

            ShowComicsViewCommand = new RelayCommand(PerformShowComicsViewCommand);
            ExecuteOperationCommand = new RelayCommand(PerformExecuteOperationCommand, CanExecuteOperationCommand);
        }

        /// <summary>
        /// Comprobar si se puede ejecutar el comando de operación.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        /// <returns>True si permite la ejecución.</returns>
        private bool CanExecuteOperationCommand(object? parameter)
        {
            if (ModoOp == ModoOperacion.NUEVO || ModoOp == ModoOperacion.EDITAR)
            {
                if (string.IsNullOrWhiteSpace(Comic?.Nombre))
                {
                    return false;
                }
                if (Comic.Nombre.Length < 2 || Comic.Nombre.Length > 150)
                {
                    return false;
                }
                if (Comic.Autor == null)
                {
                    return false;
                }
                if (Comic.Editorial == null)
                {
                    return false;
                }
                if (Comic.PrecioCompra == null ||
                    Comic.PrecioCompra < 1 ||
                    Comic.PrecioCompra > 99999999)
                {
                    return false;
                }
                if (Comic.PrecioVenta == null ||
                    Comic.PrecioVenta < 1 ||
                    Comic.PrecioVenta > 99999999)
                {
                    return false;
                }
                if (StockLocal == null ||
                    StockLocal < 0 ||
                    StockLocal > 2147483647)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Volver a la vista de cómics.</summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformShowComicsViewCommand(object? parameter = null)
        {
            _mainViewModel.CurrentChildView = new ComicsViewModel(_mainViewModel);
        }

        /// <summary>Realizar la operación correspondiente.</summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformExecuteOperationCommand(object? parameter = null)
        {
            if (ModoOp == ModoOperacion.NUEVO)
            {
                AnyadirComic();
            }
            else if (ModoOp == ModoOperacion.EDITAR)
            {
                EditarComic();
            }
            else if (ModoOp == ModoOperacion.ELIMINAR)
            {
                EliminarComic();
            }
        }

        /// <summary>
        /// Igualar las referencias de las listas de autores y editoriales
        /// con las del cómic.
        /// </summary>
        private void IgualarReferenciasListas()
        {
            if (Comic != null)
            {
                // Reenganchar la referencia del Autor
                if (Comic.Autor != null && Autores != null)
                {
                    var autorEncontrado = Autores
                        .FirstOrDefault(a => a.AutorId == Comic.Autor.AutorId);
                    
                    if (autorEncontrado != null)
                    {
                        Comic.Autor = autorEncontrado;
                    }
                }

                // Reenganchar la referencia de la Editorial
                if (Comic.Editorial != null && Editoriales != null)
                {
                    var editorialEncontrada = Editoriales
                        .FirstOrDefault(e => e.EditorialId == Comic.Editorial.EditorialId);
                    
                    if (editorialEncontrada != null)
                    {
                        Comic.Editorial = editorialEncontrada;
                    }
                }
            }
        }

        /// <summary>
        /// Añadir un cómic a la base de datos.
        /// </summary>
        private void AnyadirComic()
        {
            using (ComicsService cs = new ComicsService())
            {
                if (Comic != null)
                {
                    try
                    {
                        if (cs.AnyadirComic(Comic, StockLocal ?? 0) == 0)
                        {
                            MessageBox.Show("Cómic añadido correctamente.");
                            PerformShowComicsViewCommand();
                        }
                        else
                        {
                            MessageBox.Show("No se ha podido añadir el cómic.");
                        }
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Ha ocurrido un error al intentar añadir el cómic.");
                    }
                }
            }
        }

        /// <summary>
        /// Editar un cómic en la base de datos.
        /// </summary>
        private void EditarComic()
        {
            using (ComicsService cs = new ComicsService())
            {
                if (Comic != null)
                {
                    try
                    {
                        if (cs.EditarComic(Comic, StockLocal ?? 0) == 0)
                        {
                            MessageBox.Show("Cómic editado correctamente.");
                            PerformShowComicsViewCommand();
                        }
                        else
                        {
                            MessageBox.Show("No se ha podido editar el cómic.");
                        }
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Ha ocurrido un error al intentar editar el cómic.");
                    }
                }
            }
        }

        /// <summary>
        /// Asignar stock a 0 de un cómic para la tienda actual.
        /// </summary>
        private void EliminarComic()
        {
            using (ComicsService cs = new ComicsService())
            {
                if (Comic != null)
                {
                    try
                    {
                        if (cs.BorrarComic(Comic) == 0)
                        {
                            MessageBox.Show("Cómic eliminado correctamente.");
                            PerformShowComicsViewCommand();
                        }
                        else
                        {
                            MessageBox.Show("No se ha podido eliminar el cómic.");
                        }
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Ha ocurrido un error al intentar eliminar el cómic.");
                    }
                }
            }
        }
    }
}
