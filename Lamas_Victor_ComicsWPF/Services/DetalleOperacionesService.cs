using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>
    /// Lógica de negocio para los detalles de las operaciones.
    /// </summary>
    internal class DetalleOperacionesService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Buscar todos los detalles de operación basados en una misma operación.
        /// </summary>
        /// <param name="operacionId">El ID de la operación.</param>
        /// <returns>Lista de todos los detalles de una operación.</returns>
        public IList<DetalleOperacion> BuscarPorOperacion(int operacionId)
        {
            using (var dop = new DetalleOperacionADO())
            {
                return dop.ListarTodosPorOperacion(operacionId);
            }
        }

        /// <summary>
        /// Inserta un nuevo detalle de operación en la base de datos.
        /// </summary>
        /// <param name="detalleOperacion">Los detalles de la operación.</param>
        /// <returns>0 si no ha habido errores.</returns>
        public int Insertar(DetalleOperacion detalleOperacion)
        {
            using (var dop = new DetalleOperacionADO())
            {
                return dop.Insertar(detalleOperacion);
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
