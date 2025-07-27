using Lamas_Victor_ComicsWPF.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class DetalleOperacionADO : IDisposable
    {
        bool disposed;

        public DetalleOperacionADO()
        {
            disposed = false;
        }

        // LISTAR todas las operaciones con sus detalles
        public IList<DetalleOperacion> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.DetalleOperaciones
                    .Include(detOp => detOp.Comic)
                    .Include(detOp => detOp.Operacion)
                    .ToList();
                return data;
            }
        }

        // LISTAR todos los detalles de una operación por su ID
        public IList<DetalleOperacion> ListarTodosPorOperacion(int operacionId)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.DetalleOperaciones
                    .Include(detOp => detOp.Comic)
                    .Include(detOp => detOp.Operacion)
                    .Where(detOp => detOp.OperacionId == operacionId)
                    .ToList();
                return data;
            }
        }

        // OBJETO detalles de una operación por su ID
        public DetalleOperacion? ListarUnoPorId(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from detOp in context.DetalleOperaciones
                            where detOp.DetalleOperacionId == id
                            select detOp;
                var detalleOperacion = query.FirstOrDefault();
                return detalleOperacion;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public int Insertar(DetalleOperacion nuevo)
        {
            try
            {
                using (var context = new ComicsDbContext())
                {
                    /*bool existe = context.DetalleOperaciones.Any(
                        x => x.OperacionId == nuevo.OperacionId);

                    if (!existe)
                    {
                        context.Entry(nuevo).State = EntityState.Added;
                        context.SaveChanges();
                        return "";
                    }
                    else
                    {
                        return "Ya existen estos detalles de la operación";
                    }*/

                    context.Entry(nuevo).State = EntityState.Added;
                    context.SaveChanges();
                    return 0;
                }
            }
            catch (SqlException)
            {
                return 1;
            }
        }

        // MODIFICAR con formato manual sin EntityState
        public void Modificar(int id, DetalleOperacion modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.DetalleOperaciones.FirstOrDefault(
                    x => x.DetalleOperacionId == id
                );

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.DetalleOperacionId = modificado.DetalleOperacionId;
                    dato.OperacionId = modificado.OperacionId;
                    dato.ComicId = modificado.ComicId;
                    dato.Cantidad = modificado.Cantidad;
                    dato.Precio = modificado.Precio;
                    dato.Descuento = modificado.Descuento;

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
                var data = context.DetalleOperaciones.FirstOrDefault(
                    x => x.DetalleOperacionId == id
                );

                if (data != null)
                {
                    context.DetalleOperaciones.Remove(data);
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
