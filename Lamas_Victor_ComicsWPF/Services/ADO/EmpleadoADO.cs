using Lamas_Victor_ComicsWPF.Models;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class EmpleadoADO : IDisposable
    {
        bool disposed;

        public EmpleadoADO()
        {
            disposed = false;
        }

        // LISTAR todos los empleados
        public IList<Empleado> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Empleados
                    .Include(e => e.Operaciones)
                    .Include(e => e.Locales)
                    .ToList();
                return data;
            }
        }

        // OBJETO empleado por su ID incluyendo relación con locales
        public Empleado? ListarUnoPorId(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from c
                            in context.Empleados.Include(e => e.Locales)
                            where c.EmpleadoId == id
                            select c;
                var empleado = query.FirstOrDefault();
                return empleado;
            }
        }

        // OBJETO empleado por su NIF incluyendo relación con locales
        public Empleado? ListarUnoPorNif(string nif)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from c
                            in context.Empleados.Include(e => e.Locales)
                            where c.Nif == nif
                            select c;
                var empleado = query.FirstOrDefault();
                return empleado;
            }
        }

        // INSERTAR añadiendo relación con un local y con verificación de
        // singularidad en el NIF
        public bool Insertar(Empleado nuevo, int localId)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.Empleados.Any(
                    x => x.Nif == nuevo.Nif
                );

                if (!existe)
                {
                    var localEncontrado =
                    context.Locales.FirstOrDefault(l => l.LocalId == localId);

                    context.Empleados.Add(nuevo);
                    localEncontrado?.Empleados.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // ACTUALIZAR un empleado por su ID, incluyendo la relación con un local
        public int Actualizar(Empleado? empleadoOriginal,
            Empleado empleadoModificado, int nuevoLocalId)
        {
            try
            {
                if (empleadoOriginal != null)
                {
                    using (var context = new ComicsDbContext())
                    {
                        var dato = context.Empleados
                            .Include(e => e.Locales)
                            .FirstOrDefault(x =>
                                x.EmpleadoId == empleadoOriginal.EmpleadoId
                            );

                        if (dato != null)
                        {
                            // Evitar duplicidad NIF si se ha modificado
                            bool existeOtroConMismoNif = false;
                            if (empleadoOriginal.Nif != empleadoModificado.Nif)
                            {
                                existeOtroConMismoNif = context.Empleados.Any(x =>
                                    x.Nif == empleadoModificado.Nif &&
                                    x.EmpleadoId != empleadoModificado.EmpleadoId
                                );
                            }

                            // Si no existe ningún otro empleado con el mismo NIF
                            if (!existeOtroConMismoNif)
                            {
                                dato.Nombre = empleadoModificado.Nombre;
                                dato.Apellido = empleadoModificado.Apellido;
                                dato.Nif = empleadoModificado.Nif;
                                dato.Direccion = empleadoModificado.Direccion;
                                dato.Password = empleadoModificado.Password;
                                dato.Email = empleadoModificado.Email;
                                dato.FechaAlta = empleadoModificado.FechaAlta;
                                dato.Fotografia = empleadoModificado.Fotografia;

                                context.SaveChanges();

                                // Eliminar el empleado del antiguo local
                                int antiguoLocalId = empleadoOriginal.Locales.
                                    Select(l => l.LocalId).FirstOrDefault();
                                var localAntiguo = context.Locales.FirstOrDefault(
                                    l => l.LocalId == antiguoLocalId
                                );
                                if (localAntiguo != null)
                                {
                                    localAntiguo.Empleados.Remove(dato);
                                }

                                // Asignar al nuevo local
                                var localNuevo = context.Locales.FirstOrDefault(
                                    l => l.LocalId == nuevoLocalId
                                );
                                if (localNuevo != null)
                                {
                                    localNuevo.Empleados.Add(dato);
                                }

                                context.SaveChanges();

                                return 0;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        return 2;
                    }
                }
                return 2;
            }
            catch (Exception)
            {
                return 3;
            }
        }

        // DAR BAJA al empleado, actualizar datos y eliminar relación con local
        public bool DarBaja(Empleado empleado)
        {
            using (var context = new ComicsDbContext())
            {
                var dato = context.Empleados
                    .Include(e => e.Locales)
                    .FirstOrDefault(x => x.EmpleadoId == empleado.EmpleadoId);

                if (dato != null)
                {
                    // Desactivarlo y actualizar fecha baja
                    dato.Activo = "N";
                    dato.FechaBaja = empleado.FechaBaja;

                    context.SaveChanges();

                    // Eliminar el empleado del local al que pertenecía
                    int localId = empleado.Locales.
                        Select(l => l.LocalId).FirstOrDefault();
                    var local = context.Locales.FirstOrDefault(
                        l => l.LocalId == localId
                    );
                    if (local != null)
                    {
                        local.Empleados.Remove(dato);
                    }

                    context.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        // ELIMINAR un empleado por su ID
        public bool Eliminar(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Empleados.FirstOrDefault(
                    x => x.EmpleadoId == id
                );

                if (data != null)
                {
                    context.Empleados.Remove(data);
                    context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Número total de pedidos de un empleado en el último mes.
        /// </summary>
        /// <param name="empleadoId">(int) ID del empleado</param>
        /// <returns>Número de pedidos mensual.</returns>
        public int TotalPedidosEmpleadoUltimoMes(int empleadoId)
        {
            using (var context = new ComicsDbContext())
            {
                DateTime fechaHaceUnMes = DateTime.Now.AddMonths(-1);
                int totalPedidos = context.Operaciones
                    .Where(o => o.Empleado.EmpleadoId == empleadoId
                             && o.FechaOperacion >= fechaHaceUnMes)
                    .Count();
                return totalPedidos;
            }
        }

        /// <summary>
        /// Mayor cantidad de pedidos realizados por un empleado en el último año.
        /// </summary>
        /// <returns>Numero de pedidos anual.</returns>
        public int MayorCantidadPedidosMensualesUltimoAnyo()
        {
            using (var context = new ComicsDbContext())
            {
                DateTime fechaHaceUnAnyo = DateTime.Now.AddYears(-1);

                var mayorCantidad = context.Operaciones
                    .Where(o => o.FechaOperacion >= fechaHaceUnAnyo)
                    .GroupBy(o => new
                    {
                        o.EmpleadoId,
                        Mes = o.FechaOperacion.Month,
                        Anyo = o.FechaOperacion.Year
                    })
                    .Select(g => g.Count())
                    .OrderByDescending(c => c)
                    .FirstOrDefault();

                return mayorCantidad;
            }
        }

        /// <summary>
        /// Importe total de los pedidos realizados el día de hoy.
        /// </summary>
        /// <returns>Suma en euros de los pedidos diarios.</returns>
        public decimal? TotalImportePedidosHoyLocal(int localId)
        {
            using (var context = new ComicsDbContext())
            {
                DateTime fechaHoy = DateTime.Now.Date;
                decimal? totalImporte = context.Operaciones
                    .Where(o => o.FechaOperacion.Date == fechaHoy
                             && o.LocalId == localId)
                    .SelectMany(o => o.DetalleOperaciones)
                    .Sum(d => d.Precio);
                return totalImporte;
            }
        }

        /// <summary>
        /// Importe total de los pedidos realizados el día de hoy por un empleado.
        /// </summary>
        /// <param name="empleadoId">(int) ID del empleado.</param>
        /// <returns>
        /// Suma en euros de los pedidos diarios realizados por un empleado.
        /// </returns>
        public decimal? TotalImportePedidosHoyEmpleado(int empleadoId)
        {
            using (var context = new ComicsDbContext())
            {
                DateTime fechaHoy = DateTime.Now.Date;
                decimal? totalImporte = context.Operaciones
                    .Where(o => o.Empleado.EmpleadoId == empleadoId
                             && o.FechaOperacion.Date == fechaHoy)
                    .SelectMany(o => o.DetalleOperaciones)
                    .Sum(d => d.Precio);
                return totalImporte;
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
