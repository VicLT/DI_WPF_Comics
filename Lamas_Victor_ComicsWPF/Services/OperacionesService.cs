using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para las operaciones.</summary>
    public class OperacionesService : IDisposable
    {
        private bool disposedValue;

        /// <summary>Insertar una nueva operación.</summary>
        /// <param name="operacion">Datos de la operación.</param>
        /// <returns>0 si no ha habido errores.</returns>
        public int Insertar(Operacion operacion)
        {
            using (var oado = new OperacionADO())
            {
                return oado.Insertar(operacion);
            }
        }

        /// <summary>Buscar todas las operaciones.</summary>
        /// <returns>Lista de todas las operaciones.</returns>
        public IList<Operacion> CargarTodasLasOperaciones()
        {
            using (var oado = new OperacionADO())
            {
                return oado.ListarTodos();
            }
        }

        /// <summary>
        /// Buscar todas las operaciones de todas las editoriales.
        /// </summary>
        /// <returns>
        /// Lista con todas las editoriales y la cantidad de operaciones que tienen.
        /// </returns>
        public IList<EditorialConNumeroVentas> EditorialesConNumeroVentas()
        {
            IList<Editorial> editoriales;
            IList<EditorialConNumeroVentas> editorialesConNumeroVentas =
                new List<EditorialConNumeroVentas>();

            using (var eado = new EditorialADO())
            {
                editoriales = eado.ListarTodos();
            }
            
            // Recorrer las editoriales y buscar las operaciones de cada una
            using (var oado = new OperacionADO())
            {
                foreach (Editorial editorial in editoriales)
                {
                    editorialesConNumeroVentas.Add(
                        new EditorialConNumeroVentas()
                        {
                            nombre = editorial.Nombre,
                            numeroVentas =
                                oado.VentasPorEditorial(editorial.EditorialId)
                        }
                    );
                }
            }

            return editorialesConNumeroVentas;
        }

        /// <summary>Buscar el número total de ventas registradas.</summary>
        /// <returns>Cantidad total de ventas registradas.</returns>
        public int VentasTotales()
        {
            using (var oado = new OperacionADO())
            {
                return oado.VentasTotales();
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
