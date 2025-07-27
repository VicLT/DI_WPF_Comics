using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para los clientes.</summary>
    internal class ClientesService : IDisposable
    {
        private bool disposedValue;

        /// <summary>Listado completo de clientes.</summary>
        /// <returns>Lista observable de todos los clientes.</returns>
        public ObservableCollection<ClienteVlt> ListadoClientes()
        {
            ObservableCollection<ClienteVlt> clientesObservable =
                new ObservableCollection<ClienteVlt>();

            using (var cado = new ClienteADO())
            {
                foreach (ClienteVlt cliente in cado.ListarTodos())
                {
                    clientesObservable.Add(cliente);
                }
            }

            return clientesObservable;
        }

        /// <summary>Insertar nuevo cliente detalle operación.</summary>
        /// <param name="cliente">Cliente al que añadir la relación.</param>
        /// <param name="detalleOperacionId">
        /// Detalle operación que usaremos para relacionar con el cliente.
        /// </param>
        /// <returns>Valor del resultado para comprobar errores.</returns>
        public int AnyadirDetalleOperacionAlCliente(
            ClienteVlt? cliente, int? detalleOperacionId)
        {
            using (var cado = new ClienteADO())
            {
                return cado.AnyadirNuevaRelacion(cliente, detalleOperacionId);
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
