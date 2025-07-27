using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

/// <author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para los empleados.</summary>
    public class EmpleadosService : IDisposable
    {
        private bool disposedValue;

        /// <summary>Cargar en una IList todos los empleados.</summary>
        /// <returns>Lista de empleados.</returns>
        public IList<Empleado> CargarTodosLosEmpleados()
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.ListarTodos();
            }
        }

        /// <summary>Cargar en una lista observable todos los empleados.</summary>
        /// <returns>ObservableCollection de empleados.</returns>
        public ObservableCollection<Empleado> CargarTodosLosEmpleadosObservables()
        {
            ObservableCollection<Empleado> empleadosObservable =
                new ObservableCollection<Empleado>();

            using (var eado = new EmpleadoADO())
            {
                foreach (Empleado empleado in eado.ListarTodos())
                {
                    empleadosObservable.Add(empleado);
                }
            }
            return empleadosObservable;
        }

        /// <summary>Buscar a un empleado por su ID.</summary>
        /// <param name="empleadoId">(int) ID del empleado a buscar.</param>
        /// <returns>Empleado encontrado o null si no existe.</returns>
        public Empleado? CargarEmpleadoPorId(int empleadoId)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.ListarUnoPorId(empleadoId);
            }
        }

        /// <summary>Buscar a un empleado por su NIF.</summary>
        /// <param name="nif">(string) NIF del empleado a buscar.</param>
        /// <returns>Empleado encontrado o null si no existe.</returns>
        public Empleado? CargarEmpleadoPorNif(string nif)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.ListarUnoPorNif(nif);
            }
        }

        /// <summary>Insertar un empleado con un local.</summary>
        /// <param name="empleado">(Empleado) Empleado a insertar.</param>
        /// <param name="localId">(int)
        /// ID del local al que pertenece el nuevo empleado.
        /// </param>
        /// <returns>True si se ha insertado correctamente.</returns>
        public bool InsertarEmpleado(Empleado empleado, int localId)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.Insertar(empleado, localId);
            }
        }

        /// <summary>Actualizar datos de un empleado.</summary>
        /// <param name="empleadoOriginal">(Empleado)
        /// Empleado con los datos actuales.
        /// </param>
        /// <param name="empleadoModificado">(int)
        /// Empleado con los nuevos datos.
        /// </param>
        /// <param name="nuevoLocalId">(int)
        /// ID del local al que pertenece (o quiere pertenecer) el empleado.
        /// </param>
        /// <returns>0 si la actualización se ha realizado correctamente.</returns>
        /// <returns>1 si ya existe otro empleado con el mismo NIF.</returns>
        /// <returns>2 si no se ha encontrado al empleado.</returns>
        /// <returns>3 si se produce una excepción.</returns>
        public int ActualizarEmpleado(
            Empleado? empleadoOriginal, Empleado empleadoModificado, int nuevoLocalId)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.Actualizar(
                    empleadoOriginal, empleadoModificado, nuevoLocalId);
            }
        }

        /// <summary>Dar de baja a un empleado.</summary>
        /// <param name="empleado">(Empleado) Empleado con los datos actuales.</param>
        /// <returns>True si se ha dado de baja correctamente.</returns>
        public bool DarDeBajaEmpleado(Empleado empleado)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.DarBaja(empleado);
            }
        }

        /// <summary>Eliminar a un empleado.</summary>
        /// <param name="empleadoId">(int) ID del empleado a eliminar.</param>
        /// <returns>True si se ha eliminado correctamente.</returns>
        public bool EliminarEmpleado(int empleadoId)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.Eliminar(empleadoId);
            }
        }

        /// <summary>Comprobar credenciales login.</summary>
        /// <param name="nif">(string) NIF empleado formulario login.</param>
        /// <param name="pass">(string) Contraseña empleado formulario login.</param>
        /// <returns>0 = coincidencia completa.</returns>
        /// <returns>1 = solo coincide el NIF.</returns>
        /// <returns>2 = solo coincide la password.</returns>
        /// <returns>3 = ninguna coincidencia.</returns>
        public int comprobarCredenciales(string nif, string pass)
        {
            IList<Empleado> empleados = CargarTodosLosEmpleados();
            bool nifCorrecto = false;
            bool passCorrecta = false;
            string passCodificada = encriptacionSHA256(pass);
            
            // Recorre la lista de empleados y compara datos
            foreach (Empleado empleado in empleados)
            {
                // Sale del bucle si ambas credenciales son correctas
                if (!nifCorrecto && !passCorrecta)
                {
                    if (empleado.Nif == nif)
                    {
                        nifCorrecto = true;
                    }
                    if (empleado.Password == passCodificada)
                    {
                        passCorrecta = true;
                    }
                }
            }
            
            if (nifCorrecto && passCorrecta)
            {
                // Rellenar datos del usuario logeado para la clase singleton
                Empleado? empleado = CargarEmpleadoPorNif(nif);
                if (empleado != null)
                {
                    EmpleadoLogged.Instance.empleado = empleado;

                    if (empleado.Fotografia != null)
                    {
                        EmpleadoLogged.Instance.imagenBitMap =
                            ConversorArrayBytesEnBitMapImage(empleado.Fotografia);
                    }

                    EmpleadoLogged.Instance.localEmpleado =
                        EmpleadoLogged.Instance.empleado.Locales.FirstOrDefault();
                }

                return 0;
            }
            else if (nifCorrecto && !passCorrecta)
            {
                return 1;
            }
            else if (!nifCorrecto && passCorrecta)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        /// <summary>
        /// Número total de pedidos de un empleado en el último mes.
        /// </summary>
        /// <param name="empleadoId">(int)
        /// ID del usuario logeado.
        /// </param>
        /// <returns>
        /// Cantidad total de pedidos realizados por un usuario en el último mes.
        /// </returns>
        public int TotalPedidosEmpleadoUltimoMes(int empleadoId)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.TotalPedidosEmpleadoUltimoMes(empleadoId);
            }
        }

        /// <summary>
        /// Mayor cantidad de pedidos realizados por un empleado en el último año.
        /// </summary>
        /// <returns>
        /// Cantidad de pedidos.
        /// </returns>
        public int MayorCantidadPedidosMensualesUltimoAnyo()
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.MayorCantidadPedidosMensualesUltimoAnyo();
            }
        }

        /// <summary>
        /// Importe de los pedidos realizados el día de hoy.
        /// </summary>
        /// <returns>
        /// Importe total de todos los pedidos realizados.
        /// </returns>
        public decimal? TotalImportePedidosHoyLocal()
        {
            using (var eado = new EmpleadoADO())
            {
                var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

                if (localEmpleado != null)
                {
                    return eado.TotalImportePedidosHoyLocal(localEmpleado.LocalId);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Importe de los pedidos realizados el día de hoy por el usuario logeado.
        /// </summary>
        /// <param name="empleadoId">(int)
        /// ID del usuario logeado.
        /// </param>
        /// <returns>
        /// Importe total de todos los pedidos realizados por un usuario.
        /// </returns>
        public decimal? TotalImportePedidosHoyEmpleado(int empleadoId)
        {
            using (var eado = new EmpleadoADO())
            {
                return eado.TotalImportePedidosHoyEmpleado(empleadoId);
            }
        }

        /// <summary>Pedidos de un empleado por cada mes.</summary>
        /// <returns>Lista de empleados con sus pedidos por cada mes.</returns>
        public List<EmpleadoOperacionesMensuales> OperacionesMensualesEmpleados()
        {
            IList<Empleado> empleados = CargarTodosLosEmpleados();
            
            List<EmpleadoOperacionesMensuales> empleadosConOperacionesMensuales =
                new List<EmpleadoOperacionesMensuales>();
            
            int anyoActual = DateTime.Now.Year;

            foreach (Empleado empleado in empleados)
            {
                string nombreCompleto = empleado.Nombre + " " + empleado.Apellido;

                var empleadoConOperacionesMensuales =
                    new EmpleadoOperacionesMensuales(nombreCompleto);

                for (int mes = 1; mes <= 12; mes++) // Operaciones por mes
                {
                    empleadoConOperacionesMensuales.cantidadOperacionesMensuales[mes - 1] =
                        empleado.Operaciones
                        .Where(o => o.FechaOperacion.Month == mes) // Filtrar por mes
                        .Count();
                }

                empleadosConOperacionesMensuales.Add(empleadoConOperacionesMensuales);
            }

            return empleadosConOperacionesMensuales;
        }

        /// <summary>Encriptar texto con SHA256.</summary>
        /// <param name="pass">(string) Contraseña original en texto plano.</param>
        /// <returns>Contraseña encriptada.</returns>
        public string encriptacionSHA256(string pass)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] message = Encoding.UTF8.GetBytes(pass.Trim());
                byte[] hashValue = mySHA256.ComputeHash(message);

                return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
        }

        /// <summary>Convertir array de bytes en imagen.</summary>
        /// <param name="imagenBytes">
        /// (byte[]) Imagen hecha de un array de bytes con formato hexadecimal.
        /// </param>
        /// <returns>Imagen con formato accesible por WPF.</returns>
        public BitmapImage? ConversorArrayBytesEnBitMapImage(byte[] imagenBytes)
        {
            if (imagenBytes == null || imagenBytes.Length == 0)
                return null;

            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                MemoryStream ms = new MemoryStream(imagenBytes);
                ms.Seek(0, SeekOrigin.Begin);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "No se ha podido cargar la imagen de perfil del usuario.",
                    "ERROR",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return null;
            }
        }

        /*public BitmapImage? ConversorArrayBytesEnBitMapImage(byte[] imagenBytes)
        {
            BitmapImage bitmapImage = new BitmapImage();

            if (imagenBytes.Length > 0)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(imagenBytes))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        "No se ha podido cargar la imagen.",
                        "ERROR IMAGEN",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            
            return bitmapImage;
        }*/

        /// <summary>Convertir la imagen en un array de bytes.</summary>
        /// <param name="imagen">
        /// (BitmapImage) Imagen en formatos conocidos (.png, etc.).
        /// </param>
        /// <returns>Array de bytes hexadecimal.</returns>
        public byte[]? ConversorImagenEnBytes(BitmapImage imagen)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Creamos un encoder adecuado según el formato de la imagen
                BitmapEncoder encoder = imagen.UriSource.AbsolutePath.EndsWith(
                    ".png", StringComparison.OrdinalIgnoreCase)
                        ? new PngBitmapEncoder()
                        : new JpegBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(imagen));

                // Guardamos la imagen en el stream
                encoder.Save(ms);

                // Devolvemos el contenido del MemoryStream como un array de bytes
                return ms.ToArray();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
