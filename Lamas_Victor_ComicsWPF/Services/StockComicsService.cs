using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para el stock de cómics.</summary>
    internal class StockComicsService : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Modificar el stock de un cómic en la tienda del empleado.
        /// </summary>
        /// <param name="comicId">ID del cómic a modificar su stock.</param>
        /// <param name="cantidad">Nueva cantidad a modificar.</param>
        /// <returns>0 si no han habido errores.</returns>
        public int ModificarStockComicTienda(int comicId, int cantidad)
        {
            using (var scado = new StockComicADO())
            {
                var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

                if (localEmpleado != null)
                {
                    return scado.ActualizarCantidad(
                        comicId, localEmpleado.LocalId, cantidad);
                }
            }
            return 3;
        }

        /// <summary>
        /// Cantidad de cómics con menos de 10 unidades en stock en la tienda.
        /// </summary>
        /// <returns>
        /// Cantidad total de productos en la tienda con un stock menor a 10 ud/s.
        /// </returns>
        public int? TotalProductosMenoresDecena()
        {
            using (var scado = new StockComicADO())
            {
                var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

                if (localEmpleado != null)
                {
                    return scado.TotalProductosMenoresDecena(localEmpleado.LocalId);
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Todo el stock de cómics en la tienda del empleado.
        /// </summary>
        /// <returns>Stock de cómics asociados a una tienda.</returns>
        public ObservableCollection<StockComic> ListarStockComics()
        {
            ObservableCollection<StockComic> stockObservable =
                new ObservableCollection<StockComic>();

            using (var scado = new StockComicADO())
            {
                var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

                if (localEmpleado != null)
                {
                    foreach (StockComic stockComic in scado.ListarTodosLocal(
                        localEmpleado.LocalId))
                    {
                        stockObservable.Add(stockComic);
                    }
                }
            }

            return stockObservable;
        }

        /// <summary>Cantidad de un cómic en la tienda del empleado.</summary>
        /// <param name="comicId">(int) ID del cómic.</param>
        /// <returns>Unidades de un cómic en el local del empleado.</returns>
        public int StockComicTienda(int comicId)
        {
            using (var scado = new StockComicADO())
            {
                var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

                if (localEmpleado != null)
                {
                    return scado.StockComicTienda(comicId, localEmpleado.LocalId);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Stock de un cómic en el resto de locales ordenados por el ID del local.
        /// </summary>
        /// <param name="comicId">(int) ID del cómic.</param>
        /// <returns>Stock de un cómic en el resto de locales.</returns>
        public ObservableCollection<StockComic> StockComicRestoLocales(int comicId)
        {
            ObservableCollection<StockComic> stockObservable =
                new ObservableCollection<StockComic>();

            var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

            if (localEmpleado != null)
            {
                using (var scado = new StockComicADO())
                {
                    foreach (StockComic stockComic in scado.StockComicRestoLocales(
                        comicId, localEmpleado.LocalId))
                    {
                        stockObservable.Add(stockComic);
                    }
                }
            }

            return stockObservable;
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
