using Lamas_Victor_ComicsWPF.Models;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class TipoOperacionADO : IDisposable
    {
        bool disposed;

        public TipoOperacionADO()
        {
            disposed = false;
        }

        // LISTADO de todos los tipos de operación
        public IList<TipoOperacion> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.TiposOperacion
                    .Include(to => to.Operaciones)
                    .ToList();
                return data;
            }
        }

        // OBJETO tipoOperación por su ID
        public TipoOperacion? ListarUnoPorId(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from c in context.TiposOperacion
                            where c.TipoOperacionId == id
                            select c;
                var tipoOperacion = query.FirstOrDefault();
                return tipoOperacion;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public void Insertar(TipoOperacion nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.TiposOperacion.Any(
                    x => x.TipoOperacionId == nuevo.TipoOperacionId
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
        public void Modificar(int id, TipoOperacion modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.TiposOperacion.FirstOrDefault(
                    x => x.TipoOperacionId == id
                );

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.TipoOperacionId = modificado.TipoOperacionId;
                    dato.Descripcion = modificado.Descripcion;

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
                var data = context.TiposOperacion.FirstOrDefault(
                    x => x.TipoOperacionId == id
                );

                if (data != null)
                {
                    context.TiposOperacion.Remove(data);
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
