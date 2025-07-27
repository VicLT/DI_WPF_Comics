using Microsoft.EntityFrameworkCore;
using Lamas_Victor_ComicsWPF.Models;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class ComicADO : IDisposable
    {
        bool disposed;

        public ComicADO()
        {
            disposed = false;
        }

        // LISTAR todos los cómics
        public IList<Comic> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Comics
                    .Include(c => c.Autor)
                    .Include(c => c.DetalleOperaciones)
                    .Include(c => c.Editorial)
                    .Include(c => c.Operaciones)
                    .Include(c => c.StockComics)
                    .ToList();
                return data;
            }
        }

        // LISTAR todos los cómics de un local que tengan stock mayor a 0
        public IList<Comic> ListarTodosPorLocal(int localId)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Comics
                    .Where(c => c.StockComics
                        .Any(s => s.LocalId == localId && s.Cantidad > 0))
                    .Include(c => c.Autor)
                    .Include(c => c.DetalleOperaciones)
                    .Include(c => c.Editorial)
                    .Include(c => c.Operaciones)
                    .Include(c => c.StockComics)
                    .ToList();
                return data;
            }
        }

        // LISTAR todos los cómics ordenados
        public IList<Comic> ListarTodosOrdenados()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Comics
                    .Include(c => c.Autor)
                    .Include(c => c.DetalleOperaciones)
                    .Include(c => c.Editorial)
                    .Include(c => c.Operaciones)
                    .Include(c => c.StockComics)
                    .ToList();
                data.Sort();
                return data;
            }
        }

        // OBJETO cómic por su ID
        public Comic? ListarUnoPorId(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from c in context.Comics
                            where c.ComicId == id
                            select c;
                var comic = query.FirstOrDefault();
                return comic;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public int Insertar(Comic nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.Comics.Any(x => x.ComicId == nuevo.ComicId);

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
        public int Modificar(int comicId, Comic modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.Comics.FirstOrDefault(x => x.ComicId == comicId);
                
                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.ComicId = modificado.ComicId;
                    dato.Nombre = modificado.Nombre;
                    dato.AutorId = modificado.Autor?.AutorId;
                    dato.EditorialId = modificado.Editorial?.EditorialId;
                    dato.PrecioCompra = modificado.PrecioCompra;
                    dato.PrecioVenta = modificado.PrecioVenta;

                    context.SaveChanges();
                    return 0;
                }
                else
                {
                    /*throw new InvalidOperationException(
                        "No se ha podido realizar la modificación del registro."
                    );*/
                    return 1;
                }
            }
        }

        // BORRAR manualmente sin EntityState
        public int Borrar(int comicId)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Comics
                    .FirstOrDefault(x => x.ComicId == comicId);

                if (data != null)
                {
                    context.Comics.Remove(data);
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

        public int BorrarBis(Comic comic)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Comics
                    .FirstOrDefault(x => x.ComicId == comic.ComicId);

                if (data != null)
                {
                    context.Comics.Remove(data);
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
