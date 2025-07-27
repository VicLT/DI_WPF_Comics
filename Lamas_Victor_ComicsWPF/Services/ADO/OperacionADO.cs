using Lamas_Victor_ComicsWPF.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class OperacionADO : IDisposable
    {
        bool disposed;

        public OperacionADO()
        {
            disposed = false;
        }

        // LISTAR todas las operaciones
        public IList<Operacion> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Operaciones
                    .Include(o => o.Comic)
                    .Include(o => o.DetalleOperaciones)
                    .Include(o => o.Empleado)
                    .Include(o => o.Local)
                    .Include(o => o.MedioDePago)
                    .Include(o => o.TipoOperacion)
                    .ToList();
                return data;
            }
        }

        // OBJETO de una operación por su ID
        public Operacion? ListarUnoPorId(int id)
        {
            /*using (var context = new ComicsDbContext())
            {
                var query = from op in context.Operaciones
                            where op.OperacionId == id
                            select op;
                var operacion = query.FirstOrDefault<Operacion>();
                return operacion;
            }*/
            using (var context = new ComicsDbContext())
            {
                var operacion = context.Operaciones
                    .Include(o => o.Comic)
                    .Include(o => o.DetalleOperaciones)
                    .Include(o => o.Empleado)
                    .Include(o => o.Local)
                    .Include(o => o.MedioDePago)
                    .Include(o => o.TipoOperacion)
                    .FirstOrDefault(op => op.OperacionId == id);

                return operacion;
            }
        }

        // INSERTAR con formato sencillo EntityState
        public int Insertar(Operacion nueva)
        {
            try
            {
                using (var context = new ComicsDbContext())
                {
                    context.Entry(nueva).State = EntityState.Added;
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
        public void Modificar(int id, Operacion modificado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.Operaciones.FirstOrDefault(
                    x => x.OperacionId == id
                );

                if (dato != null)
                {
                    // No incluir PK para asegurar la integridad de la BD
                    //dato.OperacionId = modificado.OperacionId;
                    dato.MedioDePagoId = modificado.MedioDePagoId;
                    dato.TipoOperacionId = modificado.TipoOperacionId;
                    dato.ComicId = modificado.ComicId;
                    dato.LocalId = modificado.LocalId;
                    dato.FechaOperacion = modificado.FechaOperacion;
                    dato.EmpleadoId = modificado.EmpleadoId;

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
                var data = context.Operaciones.FirstOrDefault(
                    x => x.OperacionId == id
                );

                if (data != null)
                {
                    context.Operaciones.Remove(data);
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

        /// <summary>Cantidad total de ventas de una editorial.</summary>
        /// <param name="editorialId">(int) ID de la editorial.</param>
        /// <returns>Cantidad total de ventas de una editorial.</returns>
        public int VentasPorEditorial(int editorialId)
        {
            using (var context = new ComicsDbContext())
            {
                var totalVentasEditorial = context.Operaciones
                    .Where(op => op.Comic.EditorialId == editorialId
                            && op.TipoOperacionId == 2)  // 2 == Venta
                    .Count();
                    
                return totalVentasEditorial;
            }
        }

        /// <summary>Cantidad total de ventas registradas sin filtro.</summary>
        /// <returns>Cantidad total de ventas registradas sin filtro.</returns>
        public int VentasTotales()
        {
            using (var context = new ComicsDbContext())
            {
                var totalVentas = context.Operaciones
                    .Where(o => o.TipoOperacionId == 2) // 2 == Venta
                    .Count();

                return totalVentas;
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
