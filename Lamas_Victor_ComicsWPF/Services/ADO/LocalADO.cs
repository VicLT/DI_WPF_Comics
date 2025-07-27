using Lamas_Victor_ComicsWPF.Models;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class LocalADO : IDisposable
    {
        bool disposed;

        public LocalADO()
        {
            disposed = false;
        }

        // LISTAR todos los locales
        public IList<Local> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Locales
                    //.Include(l => l.Operaciones)
                    //.Include(l => l.StockComics)
                    //.Include(l => l.Empleados)
                    .ToList();
                return data;
            }
        }

        // OBJETO local por su ID
        public Local? ListarUnoPorId(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from c in context.Locales
                            where c.LocalId == id
                            select c;
                var local = query.FirstOrDefault();
                return local;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public void Insertar(Local nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.Locales.Any(
                    x => x.LocalId == nuevo.LocalId
                );

                if (!existe)
                {
                    context.Entry(nuevo).State = EntityState.Added;
                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException(
                        "No se ha podido insertar el nuevo registro."
                    );
                }
            }
        }

        // MODIFICAR con formato manual sin EntityState
        public void Modificar(int id, Local modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.Locales.FirstOrDefault(
                    x => x.LocalId == id
                );

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.LocalId = modificado.LocalId;
                    dato.Nombre = modificado.Nombre;
                    dato.Direccion = modificado.Direccion;
                    dato.Latitud = modificado.Latitud;
                    dato.Longitud = modificado.Longitud;

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

        // BORRAR manualmente sin EntityState
        public void Borrar(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Locales.FirstOrDefault(
                    x => x.LocalId == id
                );

                if (data != null)
                {
                    context.Locales.Remove(data);
                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException(
                        "No se ha podido realizar la eliminación del registro."
                    );
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
