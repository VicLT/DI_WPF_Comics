using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para los locales.</summary>
    class LocalesService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Cargar en una ObservableCollection todos los locales.
        /// </summary>
        /// <returns>Todas los locales registrados.</returns>
        public ObservableCollection<Local> CargarLocalesObservables()
        {
            ObservableCollection<Local> locales = new ObservableCollection<Local>();

            using (var lado = new LocalADO())
            {
                foreach (Local local in lado.ListarTodos())
                {
                    locales.Add(local);
                }
            }

            return locales;
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
