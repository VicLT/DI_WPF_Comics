using Lamas_Victor_ComicsWPF.Models;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class MedioDePagoADO : IDisposable
    {
        bool disposed;

        public MedioDePagoADO()
        {
            disposed = false;
        }

        // LISTAR todos los medios de pago
        public IList<MedioDePago> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.MediosDePago
                    .Include(mdp => mdp.Operaciones)
                    .ToList();
                return data;
            }
        }

        // OBJETO medioDePago por su ID
        public MedioDePago? ListarUnoPorId(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from c in context.MediosDePago
                            where c.MedioDePagoId == id
                            select c;
                var medioDePago = query.FirstOrDefault();
                return medioDePago;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public void Insertar(MedioDePago nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.MediosDePago.Any(
                    x => x.MedioDePagoId == nuevo.MedioDePagoId
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
        public void Modificar(int id, MedioDePago modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.MediosDePago.FirstOrDefault(
                    x => x.MedioDePagoId == id
                );

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.MedioDePagoId = modificado.MedioDePagoId;
                    dato.Descripcion = modificado.Descripcion;
                    dato.NombreCorto = modificado.NombreCorto;

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
                var data = context.MediosDePago.FirstOrDefault(
                    x => x.MedioDePagoId == id
                );

                if (data != null)
                {
                    context.MediosDePago.Remove(data);
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
