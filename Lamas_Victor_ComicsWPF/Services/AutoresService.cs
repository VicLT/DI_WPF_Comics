using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para los autores.</summary>
    internal class AutoresService : IDisposable
    {
        private bool disposedValue;

        /// <summary>Listado completo de autores.</summary>
        /// <returns>Lista observable de todos los autores.</returns>
        public ObservableCollection<Autor> ListadoAutores()
        {
            ObservableCollection<Autor> autoresObservable =
                new ObservableCollection<Autor>();

            using (var aado = new AutorADO())
            {
                foreach (Autor autor in aado.ListarTodos())
                {
                    autoresObservable.Add(autor);
                }
            }

            return autoresObservable;
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
