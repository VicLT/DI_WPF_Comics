using Lamas_Victor_ComicsWPF.Models;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class AutorADO : IDisposable
    {
        bool disposed;

        public AutorADO()
        {
            disposed = false;
        }

        // LISTAR todos los autores
        public IList<Autor> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Autores
                    .Include(a => a.Comics)
                    .ToList();
                return data;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public void Insertar(Autor nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.Autores.Any(x => x.AutorId == nuevo.AutorId);

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
        public void Modificar(int id, Autor modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.Autores.FirstOrDefault(x => x.AutorId == id);

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.AutorId = modificado.AutorId;
                    dato.Nombre = modificado.Nombre;
                    dato.Apellido = modificado.Apellido;
                    dato.Pais = modificado.Pais;

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
                var data = context.Autores.FirstOrDefault(x => x.AutorId == id);

                if (data != null)
                {
                    context.Autores.Remove(data);
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
