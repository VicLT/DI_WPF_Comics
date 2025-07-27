using Lamas_Victor_ComicsWPF.Models;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class EditorialADO : IDisposable
    {
        bool disposed;

        public EditorialADO()
        {
            disposed = false;
        }

        // LISTADO con todas las editoriales
        public IList<Editorial> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Editoriales
                    .Include(e => e.Comics)
                    .ToList();
                return data;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public void Insertar(Editorial nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.Editoriales.Any(
                    x => x.EditorialId == nuevo.EditorialId
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
        public void Modificar(int id, Editorial modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.Editoriales.FirstOrDefault(
                    x => x.EditorialId == id
                );

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.EditorialId = modificado.EditorialId;
                    dato.Nombre = modificado.Nombre;

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
                var data = context.Editoriales.FirstOrDefault(
                    x => x.EditorialId == id
                );

                if (data != null)
                {
                    context.Editoriales.Remove(data);
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
