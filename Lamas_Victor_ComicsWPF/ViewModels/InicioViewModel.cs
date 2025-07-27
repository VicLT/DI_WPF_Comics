using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services;
using Lamas_Victor_ComicsWPF.ViewModels.Base;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Diagnostics;
using System.Collections.ObjectModel;
using LiveChartsCore.Kernel.Sketches;
using System.Windows.Threading;
using LiveChartsCore.SkiaSharpView.VisualElements;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ViewModels
{
    /// <summary>
    /// ViewModel de la vista de Inicio.
    /// </summary>
    public class InicioViewModel : BaseViewModel
    {
        // Fields
        private DispatcherTimer? timer;
        private Empleado? empleado;
        private int totalPedidosEmpleadoUltimoMes = 0;
        private int totalProductosMenoresDecena = 0;
        private decimal? totalImportePedidosHoyLocal = 0;
        private decimal? totalImportePedidosHoyEmpleado = 0;

        private ObservableCollection<ISeries> series1 =
            new ObservableCollection<ISeries>();
        private ICartesianAxis[]? xAxes1;
        private ICartesianAxis[]? yAxes1;
        private LabelVisual? tituloGraficaLineal;

        private ObservableCollection<ISeries> series2 =
            new ObservableCollection<ISeries>();

        // Properties
        public Empleado? Empleado
        {
            get { return empleado; }
            set { empleado = value; OnPropertyChanged(); }
        }
        public int TotalPedidosEmpleadoUltimoMes
        {
            get { return totalPedidosEmpleadoUltimoMes; }
            set { totalPedidosEmpleadoUltimoMes = value; OnPropertyChanged(); }
        }
        public int TotalProductosMenoresDecena
        {
            get { return totalProductosMenoresDecena; }
            set { totalProductosMenoresDecena = value; OnPropertyChanged(); }
        }
        public decimal? TotalImportePedidosHoyLocal
        {
            get { return totalImportePedidosHoyLocal; }
            set { totalImportePedidosHoyLocal = value; OnPropertyChanged(); }
        }
        public decimal? TotalImportePedidosHoyEmpleado
        {
            get { return totalImportePedidosHoyEmpleado; }
            set { totalImportePedidosHoyEmpleado = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ISeries> Series1
        {
            get { return series1; }
            set { series1 = value; OnPropertyChanged(); }
        }
        public ICartesianAxis[]? XAxes1
        {
            get { return xAxes1; }
            set { xAxes1 = value; OnPropertyChanged(); }
        }
        public ICartesianAxis[]? YAxes1
        {
            get { return yAxes1; }
            set { yAxes1 = value; OnPropertyChanged(); }
        }
        public LabelVisual? TituloGraficaLineal
        {
            get { return tituloGraficaLineal; }
            set { tituloGraficaLineal = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ISeries> Series2
        {
            get { return series2; }
            set { series2 = value; OnPropertyChanged(); }
        }

        // Constructor
        public InicioViewModel()
        {
            // Configurar el temporizador
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += EventoTemporizador; // Evento a ejecutar cada 60 segundos
            timer.Start();

            // Datos al iniciar el ViewModel
            ActualizarDatos();
        }

        /// <summary>
        /// Método que se ejecuta cada 60 segundos para refrescar los datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventoTemporizador(object? sender, EventArgs e)
        {
            ReiniciarDatos();
            ActualizarDatos();
        }

        /// <summary>
        /// Reiniciar todos los datos susceptibles de actualización.
        /// </summary>
        private void ReiniciarDatos()
        {
            totalPedidosEmpleadoUltimoMes = 0;
            totalProductosMenoresDecena = 0;
            TotalImportePedidosHoyLocal = 0;
            totalImportePedidosHoyEmpleado = 0;
            Series1.Clear();
            Series2.Clear();
        }

        /// <summary>
        /// Actualizar los datos de la vista.
        /// </summary>
        private void ActualizarDatos()
        {
            Empleado = EmpleadoLogged.Instance.empleado;

            if (empleado != null)
            {
                using (EmpleadosService empleadosService = new EmpleadosService())
                {
                    TotalPedidosEmpleadoUltimoMes =
                        empleadosService.TotalPedidosEmpleadoUltimoMes(empleado.EmpleadoId);
                    TotalImportePedidosHoyLocal =
                        empleadosService.TotalImportePedidosHoyLocal();
                    TotalImportePedidosHoyEmpleado =
                        empleadosService.TotalImportePedidosHoyEmpleado(empleado.EmpleadoId);
                }

                using StockComicsService stockComicsService = new();
                TotalProductosMenoresDecena = stockComicsService.TotalProductosMenoresDecena() ?? 0;
            }

            GenerarGraficoColumnas();
            GenerarGraficoCircular();
        }

        /// <summary>
        /// Obtener el nombre de un mes a partir de su número.
        /// </summary>
        /// <param name="mes">(int)
        /// Número del mes (0 enero, 1 febrero, ..., 11 diciembre).
        /// </param>
        /// <returns>Nombre del mes.</returns>
        private string ObtenerNombreMes(int mes)
        {
            return new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo",
                "Junio", "Julio", "Agosto", "Septiembre", "Octubre",
                "Noviembre", "Diciembre" }[mes];
        }

        /// <summary>
        /// Parametrizar el gráfico de columnas con los pedidos mensuales de los empleados.
        /// </summary>
        private void GenerarGraficoColumnas()
        {
            // Recoger los nombres de los empleados y sus operaciones mensuales.
            // Recoger el empleado con mayor cantidad de pedidos en el último año.
            int CantidadEmpleados = 0;
            List<EmpleadoOperacionesMensuales> EmpleadosConOperacionesMensuales;
            int MayorCantidadPedidosMensualesUltimoAnyo = 0;

            using (EmpleadosService empleadosService = new EmpleadosService())
            {
                CantidadEmpleados =
                    empleadosService.OperacionesMensualesEmpleados().Count;
                EmpleadosConOperacionesMensuales =
                    empleadosService.OperacionesMensualesEmpleados();
                MayorCantidadPedidosMensualesUltimoAnyo =
                    empleadosService.MayorCantidadPedidosMensualesUltimoAnyo();
            }

            // Recoger las iniciales de los empleados y los totales anuales
            int[] totalesAnualesPorEmpleado = new int[CantidadEmpleados];
            for (int i = 0; i < CantidadEmpleados; i++)
            {
                totalesAnualesPorEmpleado[i] =
                    EmpleadosConOperacionesMensuales[i].cantidadOperacionesMensuales.Sum();
            }

            // Generar un array por cada empleado con los pedidos mensuales
            for (int empleado = 0; empleado < CantidadEmpleados; empleado++)
            {
                int[] empleadoConPedidosMensuales = new int[12];
                for (int mes = 0; mes < 12; mes++)
                {
                    empleadoConPedidosMensuales[mes] =
                        EmpleadosConOperacionesMensuales[empleado]
                        .cantidadOperacionesMensuales[mes];
                }

                Series1.Add(new LineSeries<int>
                {
                    Values = empleadoConPedidosMensuales,
                    Name = EmpleadosConOperacionesMensuales[empleado].nombreCompleto,
                    Stroke = null
                });
            }

            // Configurar título
            TituloGraficaLineal = new LabelVisual
            {
                Text = "Pedidos mensuales empleados",
                TextSize = 20,
                Padding = new LiveChartsCore.Drawing.Padding(40),
                Paint = new SolidColorPaint(SKColors.White)
            };

            // Configurar ejes
            string[] meses = new string[12];
            for (int mes = 0; mes < 12; mes++)
            {
                meses[mes] = ObtenerNombreMes(mes);
            }

            XAxes1 = new[]
            {
                new Axis
                {
                    Name = "Meses",
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    Labels = meses
                }
            };

            YAxes1 = new[]
            {
                new Axis
                {
                    Name = "Pedidos",
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    MinLimit = 0,
                    MaxLimit = MayorCantidadPedidosMensualesUltimoAnyo * 1.2
                }
            };
        }

        /// <summary>
        /// Parametrizar el gráfico circular con los porcentajes de ventas por editorial.
        /// </summary>
        private void GenerarGraficoCircular()
        {
            IList<EditorialConNumeroVentas> editoralesConNumeroVentas;
            int ventasTotales = 0;
            using (OperacionesService operacionesService = new OperacionesService())
            {
                editoralesConNumeroVentas = operacionesService.EditorialesConNumeroVentas();
                ventasTotales = operacionesService.VentasTotales();
            }

            // Calcular porcentajes de ventas por editorial sobre las totales
            List<decimal> porcentajesVentas = new List<decimal>();
            foreach (EditorialConNumeroVentas editorial in editoralesConNumeroVentas)
            {
                porcentajesVentas.Add(decimal.Round(
                    (decimal)editorial.numeroVentas / (decimal)ventasTotales * 100,
                    2,
                    MidpointRounding.ToZero)
                );
            }

            // Añadir datos a la serie del gráfico
            for (int i = 0; i < editoralesConNumeroVentas.Count; i++)
            {
                // Solo agregar la serie si el porcentaje es mayor que 0
                if (porcentajesVentas[i] > 0)
                {
                    Series2.Add(new PieSeries<decimal>
                    {
                        Values = new List<decimal> { porcentajesVentas[i] },
                        Name = editoralesConNumeroVentas[i].nombre,
                        DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue}%",
                        IsVisible = true
                    });
                }
            }
        }
    }
}
