using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para los medios de pago.</summary>
    internal class MediosDePagoService : IDisposable
    {
        private bool disposedValue;

        /// <summary>Listado completo de medios de pago.</summary>
        /// <returns>Lista observable de todos los medios de pago.</returns>
        public ObservableCollection<MedioDePago> ListadoMediosDePago()
        {
            ObservableCollection<MedioDePago> mediosDePagoObservable =
                new ObservableCollection<MedioDePago>();

            using (var mpado = new MedioDePagoADO())
            {
                foreach (MedioDePago medioPago in mpado.ListarTodos())
                {
                    mediosDePagoObservable.Add(medioPago);
                }
            }

            return mediosDePagoObservable;
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
