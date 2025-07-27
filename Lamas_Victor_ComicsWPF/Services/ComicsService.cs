using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services.ADO;
using System.Collections.ObjectModel;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services
{
    /// <summary>Lógica de negocio para los cómics.</summary>
    class ComicsService : IDisposable
    {
        private bool disposedValue;

        /// <summary>Cargar todos los cómics disponibles.</summary>
        /// <returns>Lista de todos los cómics.</returns>
        public ObservableCollection<Comic> CargarComicsObservables()
        {
            ObservableCollection<Comic> comicsObservable =
                new ObservableCollection<Comic>();

            using (var cado = new ComicADO())
            {
                foreach (Comic comic in cado.ListarTodos())
                {
                    comicsObservable.Add(comic);
                }
            }
            return comicsObservable;
        }

        /// <summary>
        /// Cargar todos los cómics disponibles en el local del empleado.
        /// </summary>
        /// <returns>Lista de cómics de un local.</returns>
        public ObservableCollection<Comic> CargarComicsPorLocalObservables()
        {
            ObservableCollection<Comic> comicsObservable =
                new ObservableCollection<Comic>();

            var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

            using (var cado = new ComicADO())
            {
                if (localEmpleado != null)
                {
                    foreach (Comic comic in cado.ListarTodosPorLocal(
                        localEmpleado.LocalId))
                    {
                        comicsObservable.Add(comic);
                    }
                }
            }
            return comicsObservable;
        }

        /// <summary>Buscar un cómic por su ID.</summary>
        /// <param name="comicId">(int) ID del cómic a buscar.</param>
        /// <returns>Cómic encontrado o null si no existe.</returns>
        public Comic? CargarComicPorId(int comicId)
        {
            using (var cado = new ComicADO())
            {
                return cado.ListarUnoPorId(comicId);
            }
        }

        /// <summary>Añadir un cómic a la base de datos.</summary>
        /// <param name="comic">Cómic a añadir.</param>
        /// <param name="stockLocal">Stock del cómic en el local.</param>
        /// <returns>0 si no hubieron errores.</returns>
        public int AnyadirComic(Comic comic, int stockLocal)
        {
            using (var cado = new ComicADO())
            {
                if (cado.Insertar(comic) == 0)
                {
                    using (var scado = new StockComicADO())
                    {
                        var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

                        if (localEmpleado != null)
                        {
                            return scado.Insertar(new StockComic(
                                comic.ComicId,
                                localEmpleado.LocalId,
                                stockLocal));
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Editar un cómic y su stock en el local del empleado.
        /// </summary>
        /// <param name="comic">Cómic a editar.</param>
        /// <param name="nuevoStock">Cantidad para modificar.</param>
        /// <returns>0 si no hubieron errores.</returns>
        public int EditarComic(Comic comic, int nuevoStock)
        {
            using (var cado = new ComicADO())
            {
                if (cado.Modificar(comic.ComicId, comic) == 0)
                {
                    using (var scado = new StockComicADO())
                    {
                        var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

                        if (localEmpleado != null)
                        {
                            return scado.ModificarCantidad(
                                comic.ComicId,
                                localEmpleado.LocalId,
                                nuevoStock);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>Vaciar el stock de un cómic.</summary>
        /// <param name="comicId">ID del cómic.</param>
        /// <returns>
        /// 0 si se ha vaciado el stock, 1 si ha ocurrido algún error.
        /// </returns>
        public int BorrarComic(Comic comic)
        {
            var localEmpleado = EmpleadoLogged.Instance.localEmpleado;

            if (localEmpleado != null)
            {
                using (var scado = new StockComicADO())
                {
                    return scado.EliminarStockComicLocal(
                        comic.ComicId, localEmpleado.LocalId);
                }
            }
            else
            {
                return 1;
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
