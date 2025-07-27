using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services;
using Lamas_Victor_ComicsWPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ViewModels
{
    public class OperacionesViewModel : BaseViewModel
    {
        // Fields
        public enum ModoOperacion
        {
            COMPRAR,
            VENDER
        }
        private ModoOperacion? modoOp = null;
        private string titulo = "SELECCIONE OPERACIÓN";
        private DataRowView? selectedComic;
        private DataRowView? selectedProducto;
        private ClienteVlt? selectedCliente;
        private MedioDePago? selectedMedioDePago;
        private ObservableCollection<Comic>? comics;
        private ObservableCollection<ClienteVlt>? clientes;
        private ObservableCollection<ClienteVlt>? clientesFiltrados;
        private ObservableCollection<StockComic>? stockComicsLocal;
        private ObservableCollection<MedioDePago>? mediosDePago;
        private int stockComicLocal;
        private string busqueda = "Buscar por nombre, apellidos, NIF y/o dirección.";
        private DataView comicsConStock = new DataView();
        private DataView carritoDataView = new DataView();
        private decimal totalSinIva = 0;
        private decimal totalConIva = 0;

        // Properties
        public ModoOperacion? ModoOp
        {
            get { return modoOp; }
            set { modoOp = value; OnPropertyChanged(); }
        }
        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; OnPropertyChanged(); }
        }
        public DataRowView? SelectedComic
        {
            get { return selectedComic; }
            set { selectedComic = value; OnPropertyChanged(); }
        }
        public ClienteVlt? SelectedCliente
        {
            get { return selectedCliente; }
            set { selectedCliente = value; OnPropertyChanged(); }
        }
        public DataRowView? SelectedProducto
        {
            get { return selectedProducto; }
            set { selectedProducto = value; OnPropertyChanged(); }
        }
        public MedioDePago? SelectedMedioDePago
        {
            get { return selectedMedioDePago; }
            set { selectedMedioDePago = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Comic>? Comics
        {
            get { return comics; }
            set { comics = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ClienteVlt>? Clientes
        {
            get { return clientes; }
            set { clientes = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ClienteVlt>? ClientesFiltrados
        {
            get { return clientesFiltrados; }
            set { clientesFiltrados = value; OnPropertyChanged(); }
        }
        public ObservableCollection<StockComic>? StockComicsLocal
        {
            get { return stockComicsLocal; }
            set { stockComicsLocal = value; OnPropertyChanged(); }
        }
        public ObservableCollection<MedioDePago>? MediosDePago
        {
            get { return mediosDePago; }
            set { mediosDePago = value; OnPropertyChanged(); }
        }
        public int StockComicLocal
        {
            get { return stockComicLocal; }
            set { stockComicLocal = value; OnPropertyChanged(); }
        }
        public string Busqueda
        {
            get { return busqueda; }
            set
            {
                busqueda = value;
                OnPropertyChanged();
                PerformFilterClientes();
            }
        }
        public DataView ComicsConStock
        {
            get { return comicsConStock; }
            set { comicsConStock = value; OnPropertyChanged(); }
        }
        public DataView CarritoDataView
        {
            get { return carritoDataView; }
            set { carritoDataView = value; OnPropertyChanged(); }
        }
        public decimal TotalSinIva
        {
            get
            {
                decimal totalSinIva = 0;
                if (CarritoDataView != null && CarritoDataView.Table != null)
                {
                    foreach (DataRow row in CarritoDataView.Table.Rows)
                    {
                        totalSinIva += (decimal)row["Total"];
                    }
                }
                return totalSinIva;
            }
            set { totalSinIva = value; OnPropertyChanged(); }
        }
        public decimal TotalConIva
        {
            get
            {
                return TotalSinIva + (TotalSinIva * 0.21m);
            }
            set { totalConIva = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand LoadCommand { get; }
        public ICommand FilterComicsCommand { get; }
        public ICommand SelectedComicChangedCommand { get; }
        public ICommand SelectedProductoChangedCommand { get; }
        public ICommand PrepararOperacionCommand { get; }
        public ICommand ConfirmarOperacionCommand { get; }

        // Constructor
        public OperacionesViewModel()
        {
            LoadCommand = new RelayCommand(PerformCargarDatos);
            FilterComicsCommand = new RelayCommand(PerformFilterClientes);
            SelectedComicChangedCommand = new RelayCommand(
                PerformSelectedComicChangedCommand);
            SelectedProductoChangedCommand = new RelayCommand(
                PerformSelectedProductoChangedCommand);
            PrepararOperacionCommand = new RelayCommand(
                PerformPrepararOperacion);
            ConfirmarOperacionCommand = new RelayCommand(
                PerformConfirmarOperacion, canExecuteConfirmar);

            CargarTablaCarrito();
        }

        /// <summary>
        /// Comprueba si se puede ejecutar la operación de compra o venta.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        /// <returns>True si permite la ejecución.</returns>
        private bool canExecuteConfirmar(object? parameter)
        {
            if (ModoOp == ModoOperacion.COMPRAR
                && CarritoDataView.Table != null
                && CarritoDataView.Table.Rows.Count > 0
                && SelectedMedioDePago != null
                && !BloquearStockDisponible()
                && !BloquearDescuento())
            {
                return true;
            }
            if (ModoOp == ModoOperacion.VENDER
                && SelectedCliente != null
                && CarritoDataView.Table != null
                && CarritoDataView.Table.Rows.Count > 0
                && SelectedMedioDePago != null
                && !BloquearStockDisponible()
                && !BloquearDescuento())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prepara el formulario para realizar una operación.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformPrepararOperacion(object? parameter = null)
        {
            if (parameter != null)
            {
                Titulo = parameter.ToString() ?? string.Empty;

                if (parameter.ToString() == "COMPRAR")
                {
                    ModoOp = ModoOperacion.COMPRAR;
                    LimpiarFormulario();
                }
                else if (parameter.ToString() == "VENDER")
                {
                    ModoOp = ModoOperacion.VENDER;
                    using (var cs = new ClientesService())
                    {
                        Clientes = cs.ListadoClientes();
                        ClientesFiltrados = new ObservableCollection<ClienteVlt>(Clientes);
                        Busqueda = "Buscar por nombre, apellidos, NIF y/o dirección.";
                        CarritoDataView?.Table?.Clear();
                        SelectedMedioDePago = null;
                        TotalConIva = 0;
                        TotalSinIva = 0;
                    }
                }
            }
        }

        /// <summary>Resetea los campos del formulario.</summary>
        private void LimpiarFormulario()
        {
            Clientes = new ObservableCollection<ClienteVlt>();
            ClientesFiltrados = new ObservableCollection<ClienteVlt>();
            Busqueda = "Buscar por nombre, apellidos, NIF y/o dirección.";
            CarritoDataView?.Table?.Clear();
            SelectedMedioDePago = null;
            TotalConIva = 0;
            TotalSinIva = 0;
        }

        /// <summary>Realiza la operación de compra o venta de cómics.</summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformConfirmarOperacion(object? parameter = null)
        {
            MessageBoxResult result = MessageBox.Show(
                "¿Está seguro de que desea realizar la operación?",
                "Confirmar operación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (ModoOp == ModoOperacion.COMPRAR)
                {
                    EjecutarCompra();
                }
                else if (ModoOp == ModoOperacion.VENDER)
                {
                    EjecutarVenta();
                }

                // Actualizar stock
                using (var scs = new StockComicsService())
                {
                    StockComicsLocal = scs.ListarStockComics();
                }
                CargarTablaComics();
            }
        }

        /// <summary>
        /// Realiza todas las inserciones oportunas en la BD para llevar a cabo
        /// una operación de compra.
        /// </summary>
        private void EjecutarCompra()
        {
            int errores = 0;

            if (SelectedMedioDePago != null
                && CarritoDataView.Table != null
                && CarritoDataView.Table.Rows.Count > 0)
            {
                // Generar e insertar la operación
                Operacion nuevaOperacion = new Operacion(
                    SelectedMedioDePago.MedioDePagoId,
                    1, // Comprar
                    1, // Simbólico
                    EmpleadoLogged.Instance.localEmpleado?.LocalId ?? 0,
                    DateTime.Now,
                    EmpleadoLogged.Instance.empleado?.EmpleadoId ?? 0
                );

                using (var os = new OperacionesService())
                {
                    errores += os.Insertar(nuevaOperacion);
                }

                // Insertar detalles operación si no hay errores
                if (errores == 0)
                {
                    using (var dop = new DetalleOperacionesService())
                    {
                        foreach (DataRow row in CarritoDataView.Table.Rows)
                        {
                            errores += dop.Insertar(
                                new DetalleOperacion(
                                    nuevaOperacion.OperacionId,
                                    (int)row["ComicId"],
                                    (int)row["Cantidad"],
                                    (decimal)row["Precio"],
                                    (decimal)row["Descuento"]));
                        }
                    }
                }

                // Actualizar (sumar) stock tienda si no hay errores
                if (errores == 0)
                {
                    using (var scs = new StockComicsService())
                    {
                        foreach (DataRow row in CarritoDataView.Table.Rows)
                        {
                            errores +=
                                scs.ModificarStockComicTienda(
                                    (int)row["ComicId"],
                                    (int)row["Cantidad"]);
                        }
                    }
                }

                // Comprobar errores de todos los cambios en la BD
                if (errores > 0)
                {
                    MessageBox.Show("Error al realizar la operación.");
                }
                else
                {
                    MessageBox.Show("Operación realizada con éxito.");
                    LimpiarFormulario();
                    ModoOp = null;
                }
            }
            else
            {
                MessageBox.Show(
                    "Seleccione un medio de pago y añada productos al carrito.");
            }
        }

        /// <summary>
        /// Realiza todas las inserciones oportunas en la BD para llevar a cabo
        /// una operación de venta.
        /// </summary>
        private void EjecutarVenta()
        {
            int errores = 0;

            if (SelectedMedioDePago != null
                && CarritoDataView.Table != null
                && CarritoDataView.Table.Rows.Count > 0
                && SelectedCliente != null)
            {
                // Generar e insertar la operación
                Operacion nuevaOperacion = new Operacion(
                    SelectedMedioDePago.MedioDePagoId,
                    2, // Vender
                    1, // Simbólico
                    EmpleadoLogged.Instance.localEmpleado?.LocalId ?? 0,
                    DateTime.Now,
                    EmpleadoLogged.Instance.empleado?.EmpleadoId ?? 0
                );

                using (var os = new OperacionesService())
                {
                    errores += os.Insertar(nuevaOperacion);
                }

                // Insertar detalles operación si no hay errores
                if (errores == 0)
                {
                    using (var dop = new DetalleOperacionesService())
                    {
                        foreach (DataRow row in CarritoDataView.Table.Rows)
                        {
                            errores += dop.Insertar(
                                new DetalleOperacion(
                                    nuevaOperacion.OperacionId,
                                    (int)row["ComicId"],
                                    (int)row["Cantidad"],
                                    (decimal)row["Precio"],
                                    (decimal)row["Descuento"]));
                        }
                    }
                }

                // Actualizar (restar) stock tienda si no hay errores
                if (errores == 0)
                {
                    using (var scs = new StockComicsService())
                    {
                        foreach (DataRow row in CarritoDataView.Table.Rows)
                        {
                            errores +=
                                scs.ModificarStockComicTienda(
                                    (int)row["ComicId"],
                                    -(int)row["Cantidad"]);
                        }
                    }
                }

                // Añadir relación entre el cliente y los detalles de la
                // operación si todos los cambios anteriores han sido correctos
                if (errores == 0)
                {
                    // Buscamos todos los detalles que acabamos de insertar
                    using (var dop = new DetalleOperacionesService())
                    {
                        IList<DetalleOperacion> detallesOperacion =
                            dop.BuscarPorOperacion(nuevaOperacion.OperacionId);

                        if (detallesOperacion != null)
                        {
                            // Insertamos las relaciones
                            using (var cs = new ClientesService())
                            {
                                foreach (var detOp in detallesOperacion)
                                {
                                    errores += cs.AnyadirDetalleOperacionAlCliente(
                                        SelectedCliente, detOp.DetalleOperacionId);
                                }
                            }
                        }
                    }
                }

                // Comprobar errores de todos los cambios en la BD
                if (errores > 0)
                {
                    MessageBox.Show("Error al realizar la operación.");
                }
                else
                {
                    MessageBox.Show("Operación realizada con éxito.");
                    LimpiarFormulario();
                    ModoOp = null;
                }
            }
            else
            {
                MessageBox.Show(
                    "Seleccione un cliente, un medio de pago y añada "
                    + "productos al carrito.");
            }
        }

        /// <summary>Carga los datos del formulario.</summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformCargarDatos(object? parameter = null)
        {
            using (var cs = new ComicsService())
            {
                Comics = cs.CargarComicsObservables();
            }
            using (var scs = new StockComicsService())
            {
                StockComicsLocal = scs.ListarStockComics();
            }
            using (var mdp = new MediosDePagoService())
            {
                MediosDePago = mdp.ListadoMediosDePago();
            }

            CargarTablaComics();
            CargarTablaCarrito();
        }

        /// <summary>Monta la tabla vacía del carrito de la compra.</summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void CargarTablaCarrito()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ComicId", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Descuento", typeof(decimal));

            DataColumn totalColumn = new DataColumn("Total", typeof(decimal));
            totalColumn.Expression =
                "[Precio] * [Cantidad] - (([Precio] * [Cantidad]) * ([Descuento] / 100))";
            dt.Columns.Add(totalColumn);

            // Suscribirse a los cambios de la tabla
            dt.RowChanged += (s, e) =>
            {
                OnPropertyChanged("TotalSinIva");
                OnPropertyChanged("TotalConIva");
            };
            dt.RowDeleted += (s, e) =>
            {
                OnPropertyChanged("TotalSinIva");
                OnPropertyChanged("TotalConIva");
            };

            CarritoDataView = dt.DefaultView;
        }

        /// <summary>Monta y carga con todos los datos la tabla de cómics.</summary>
        private void CargarTablaComics()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ComicId", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Autor", typeof(string));
            dt.Columns.Add("Editorial", typeof(string));
            dt.Columns.Add("PCompra", typeof(decimal));
            dt.Columns.Add("PVenta", typeof(decimal));
            dt.Columns.Add("StockLocal", typeof(int));

            if (Comics != null && StockComicsLocal != null)
            {
                var query = from comic in Comics
                            join stock in StockComicsLocal on comic.ComicId equals stock.ComicId into stockJoin
                            from stock in stockJoin.DefaultIfEmpty()
                            select new
                            {
                                ComicId = comic.ComicId,
                                Nombre = comic.Nombre ?? string.Empty,
                                Autor = comic.Autor != null ?
                                    comic.Autor.NombreCompleto : string.Empty,
                                Editorial = comic.Editorial != null ?
                                    comic.Editorial.Nombre : string.Empty,
                                PCompra = comic.PrecioCompra != null ?
                                    comic.PrecioCompra : 0,
                                PVenta = comic.PrecioVenta != null ?
                                    comic.PrecioVenta : 0,
                                StockLocal = (stock != null && stock.Cantidad != null) ?
                                    stock.Cantidad : 0
                            };

                foreach (var item in query)
                {
                    dt.Rows.Add(item.ComicId, item.Nombre, item.Autor,
                        item.Editorial, item.PCompra, item.PVenta, item.StockLocal);
                }

                ComicsConStock = dt.DefaultView;
            }
        }

        /// <summary>
        /// Filtra los clientes por nombre, apellidos, NIF y/o dirección.
        /// </summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformFilterClientes(object? parameter = null)
        {
            if (Clientes != null)
            {
                if (string.IsNullOrWhiteSpace(Busqueda) ||
                    Busqueda == "Buscar por nombre, apellidos, NIF y/o dirección.")
                {
                    ClientesFiltrados = new ObservableCollection<ClienteVlt>(Clientes);
                }
                else
                {
                    var filtro = Busqueda.ToLower();

                    ClientesFiltrados = new ObservableCollection<ClienteVlt>(
                        Clientes.Where(c =>
                            (c.Nombre?.ToLower().Contains(filtro) ?? false) ||
                            (c.Apellido?.ToLower().Contains(filtro) ?? false) ||
                            (c.Nif?.ToLower().Contains(filtro) ?? false) ||
                            (c.Direccion?.ToLower().Contains(filtro) ?? false)
                        ).ToList());
                }
            }
        }

        /// <summary>Añade un cómic al carrito de la compra.</summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformSelectedComicChangedCommand(object? parameter = null)
        {
            if ((ModoOp == ModoOperacion.VENDER ||
                 ModoOp == ModoOperacion.COMPRAR)
                && SelectedComic != null && CarritoDataView?.Table != null)
            {
                DataTable dt = CarritoDataView.Table;
                DataRow newRow = dt.NewRow();
                int cantidad = 1;
                decimal precio = 0;

                newRow["ComicId"] = SelectedComic["ComicId"];
                newRow["Nombre"] = SelectedComic["Nombre"];
                newRow["Cantidad"] = cantidad;
                if (ModoOp == ModoOperacion.COMPRAR)
                {
                    precio = (decimal)SelectedComic["PCompra"];
                }
                else if (ModoOp == ModoOperacion.VENDER)
                {
                    precio = (decimal)SelectedComic["PVenta"];
                }
                newRow["Precio"] = precio;
                newRow["Descuento"] = 0;
                newRow["Total"] = precio * cantidad;

                // Comprobar que no existen duplicaciones
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ComicId"].ToString() == newRow["ComicId"].ToString())
                    {
                        return;
                    }
                }

                dt.Rows.Add(newRow);
            }
        }

        /// <summary>Elimina un producto del carrito de la compra.</summary>
        /// <param name="parameter">Valor parámetro command de la vista.</param>
        private void PerformSelectedProductoChangedCommand(object? parameter = null)
        {
            if (SelectedProducto != null && CarritoDataView?.Table != null)
            {
                DataTable dt = CarritoDataView.Table;
                int posicionFila = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["ComicId"].ToString() ==
                        SelectedProducto["ComicId"].ToString())
                    {
                        posicionFila = dt.Rows.IndexOf(row);
                        break;
                    }
                }

                dt.Rows.RemoveAt(posicionFila);
            }
        }

        /// <summary>
        /// Bloquea la venta si no hay stock suficiente en la tienda.
        /// </summary>
        /// <returns>True si la cantidad a operar no es adecuada.</returns>
        private bool BloquearStockDisponible()
        {
            bool bloquear = false;

            if (CarritoDataView.Table != null
                && CarritoDataView?.Table.Rows.Count > 0
                && StockComicsLocal != null)
            {
                foreach (DataRow row in CarritoDataView.Table.Rows)
                {
                    int comicId = (int)row["ComicId"];
                    int cantidad = (int)row["Cantidad"];
                    int stockLocal = 0;

                    // Busca coincidencia entre el cómic añadido y su stock disponible
                    foreach (StockComic stock in StockComicsLocal)
                    {
                        if (stock.ComicId == comicId)
                        {
                            stockLocal = stock.Cantidad ?? 0;
                            break; // Sale del bucle secundario
                        }
                    }

                    if (ModoOp == ModoOperacion.COMPRAR && cantidad < 1)
                    {
                        bloquear = true;
                        break; // Sale del bucle principal
                    }
                    else if (ModoOp == ModoOperacion.VENDER &&
                        (cantidad < 1 || cantidad > stockLocal))
                    {
                        bloquear = true;
                        break; // Sale del bucle principal
                    }
                }
            }

            return bloquear;
        }

        /// <summary>
        /// Bloquea la compra/venta si se exceden los límites para el descuento.
        /// </summary>
        /// <returns>True si el descuento no es el correcto.</returns>
        private bool BloquearDescuento()
        {
            bool bloquear = false;

            if (CarritoDataView.Table != null
                && CarritoDataView?.Table.Rows.Count > 0)
            {
                foreach (DataRow row in CarritoDataView.Table.Rows)
                {
                    decimal descuento = (decimal)row["Descuento"];

                    if (descuento < 0 || descuento > 100)
                    {
                        bloquear = true;
                        break;
                    }
                }
            }
            
            return bloquear;
        }
    }
}
