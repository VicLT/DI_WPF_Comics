using Lamas_Victor_ComicsWPF.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class StockComicADO : IDisposable
    {
        bool disposed;

        public StockComicADO()
        {
            disposed = false;
        }

        // LISTAR todo el stock de cómics
        public IList<StockComic> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.StockComics
                    .Include(c => c.Comic)
                    .Include(c => c.Local)
                    .ToList();
                return data;
            }
        }

        // LISTAR todo el stock de cómics de una tienda
        public IList<StockComic> ListarTodosLocal(int localId)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.StockComics
                    .Include(c => c.Comic)
                    .Include(c => c.Local)
                    .Where(x => x.LocalId == localId)
                    .ToList();
                
                return data;
            }
        }

        // BUSCAR uno por ID
        public bool ExisteStock(int comicId)
        {
            using (var context = new ComicsDbContext())
            {
                return context.StockComics.Any(x => x.ComicId == comicId);
            }
        }

        // INSERTAR con formato sencillo EntityState
        public int Insertar(StockComic nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.StockComics.Any(
                    x => x.StockComicId == nuevo.StockComicId
                );

                if (!existe)
                {
                    context.Entry(nuevo).State = EntityState.Added;
                    context.SaveChanges();
                    return 0;
                }
                else
                {
                    /*throw new InvalidOperationException(
                        "No se ha podido insertar el nuevo registro."
                    );*/
                    return 1;
                }
            }
        }

        // MODIFICAR con formato manual sin EntityState
        public void Modificar(int id, StockComic modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.StockComics.FirstOrDefault(
                    x => x.StockComicId == id
                );

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.StockComicId = modificado.StockComicId;
                    dato.ComicId = modificado.ComicId;
                    dato.LocalId = modificado.LocalId;
                    dato.Cantidad = modificado.Cantidad;

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException(
                        "No se ha podido realizar la modificación del registro."
                    );
                }
            }
        }

        // MODIFICAR cantidad absoluta
        public int ModificarCantidad(int comicId, int localId, int nuevaCantidad)
        {
            try
            {
                using (var context = new ComicsDbContext())
                {
                    var dato = context.StockComics.FirstOrDefault(
                        x => x.ComicId == comicId && x.LocalId == localId
                    );

                    if (dato != null)
                    {
                        dato.Cantidad = nuevaCantidad;
                        context.SaveChanges();
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch (SqlException)
            {
                return 1;
            }
        }

        // SUMAR o RESTAR cantidad al stock de un cómic de una tienda
        public int ActualizarCantidad(int comicId, int localId, int cantidad)
        {
            try
            {
                using (var context = new ComicsDbContext())
                {
                    var dato = context.StockComics.FirstOrDefault(
                        x => x.ComicId == comicId && x.LocalId == localId
                    );

                    if (dato != null)
                    {
                        dato.Cantidad = dato.Cantidad + cantidad;
                        context.SaveChanges();
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch (SqlException)
            {
                return 2;
            }
        }

        // BORRAR manualmente sin EntityState
        public int Borrar(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.StockComics.FirstOrDefault(
                    x => x.StockComicId == id
                );

                if (data != null)
                {
                    context.StockComics.Remove(data);
                    context.SaveChanges();
                    return 0;
                }
                else
                {
                    /*throw new InvalidOperationException(
                        "No se ha podido realizar la eliminación del registro."
                    );*/
                    return 1;
                }
            }
        }

        // BORRAR manualmente sin EntityState
        public int BorrarStockTodosLocales(int comicId)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.StockComics
                    .Where(x => x.ComicId == comicId)
                    .ToList();

                if (data.Any())
                {
                    context.StockComics.RemoveRange(data);
                    context.SaveChanges();
                    return 0;
                }
                else
                {
                    /*throw new InvalidOperationException(
                        "No se ha podido realizar la eliminación del registro."
                    );*/
                    return 1;
                }
            }
        }

        // ELIMINAR stock cómic de un local
        public int EliminarStockComicLocal(int comicId, int localId)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context
                    .StockComics
                    .FirstOrDefault(x => x.ComicId == comicId &&
                                    x.LocalId == localId);
                
                if (data != null)
                {
                    context.StockComics.Remove(data);
                    context.SaveChanges();
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Total de productos con menos de 10 unidades en stock en un local específico.
        /// </summary>
        /// <param name="localId">(int) ID del local.</param>
        /// <returns>
        /// Cantidad total de cómics en el local con menos de 10 unidades en stock.
        /// </returns>
        public int TotalProductosMenoresDecena(int localId)
        {
            using (var context = new ComicsDbContext())
            {
                int totalProductos = context.StockComics
                    .Where(s => s.Cantidad < 10 && s.LocalId == localId)
                    .Count();

                return totalProductos;
            }
        }

        /// <summary>Stock de un cómic en un local específico.</summary>
        /// <param name="comicId">(int) ID del cómic.</param>
        /// <param name="localId">(int) ID del local.</param>
        /// <returns>Unidades de un cómic en un local.</returns>
        public int StockComicTienda(int comicId, int localId)
        {
            using (var context = new ComicsDbContext())
            {
                int stock = context.StockComics
                    .Where(s => s.ComicId == comicId && s.LocalId == localId)
                    .Select(s => s.Cantidad)
                    .FirstOrDefault() ?? 0;

                return stock;
            }
        }

        /// <summary>
        /// Stock de un cómic en el resto de locales ordenados por el ID del local.
        /// </summary>
        /// <param name="comicId">(int) ID del cómic.</param>
        /// <param name="localId">(int) ID del local.</param>
        /// <returns>Lista de stock de un cómic en el resto de locales.</returns>
        public IList<StockComic> StockComicRestoLocales(int comicId, int localId)
        {
            using (var context = new ComicsDbContext())
            {
                var stock = context.StockComics
                    .Where(s => s.ComicId == comicId && s.LocalId != localId)
                    .OrderBy(s => s.LocalId)
                    .ToList();
                return stock;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //Liberar recursos no manejados como ficheros, conexiones a bd, etc.
            }

            disposed = true;
        }
    }
}
