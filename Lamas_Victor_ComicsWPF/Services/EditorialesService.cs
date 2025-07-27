using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para las editoriales.</summary>
    class EditorialesService : IDisposable
    {
        private bool disposedValue;

        /// <summary>Cargar en una IList todas las editoriales.</summary>
        /// <returns>Lista de editoriales.</returns>
        public IList<Editorial> CargarTodasLasEditoriales()
        {
            using (var eado = new EditorialADO())
            {
                return eado.ListarTodos();
            }
        }

        /// <summary>
        /// Cargar en una ObservableCollection todas las editoriales.
        /// </summary>
        /// <returns>Todas las editoriales registradas.</returns>
        public ObservableCollection<Editorial> ListarEditorialesObservable()
        {
            ObservableCollection<Editorial> editoriales = new ObservableCollection<Editorial>();

            using (var eado = new EditorialADO())
            {
                foreach (Editorial editorial in eado.ListarTodos())
                {
                    editoriales.Add(editorial);
                }
            }

            return editoriales;
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
