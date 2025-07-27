using Lamas_Victor_ComicsWPF.Models;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Services.ADO
{
    public class ClienteADO : IDisposable
    {
        bool disposed;

        public ClienteADO()
        {
            disposed = false;
        }

        // LISTAR todos los clientes
        public IList<ClienteVlt> ListarTodos()
        {
            using (var context = new ComicsDbContext())
            {
                var data = context.Clientes
                    .Include(c => c.DetalleOperaciones)
                    .ToList();
                return data;
            }
        }

        // OBJETO cliente por su ID
        public ClienteVlt? ListarUnoPorId(int id)
        {
            using (var context = new ComicsDbContext())
            {
                var query = from c
                            in context.Clientes.Include(c => c.DetalleOperaciones)
                            where c.ClienteId == id
                            select c;
                var cliente = query.FirstOrDefault();
                return cliente;
            }
        }

        // INSERTAR cliente con verificación de singularidad en el NIF sin
        // relación con ningún detalle_operaciones. Solo datos del propio cliente
        public bool Insertar(ClienteVlt nuevo)
        {
            using (var context = new ComicsDbContext())
            {
                bool existe = context.Clientes.Any(
                    x => x.Nif == nuevo.Nif
                );

                if (!existe)
                {
                    context.Clientes.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // AÑADIR relación cliente <-> detalle_operacion (DO)
        public int AnyadirNuevaRelacion(ClienteVlt? cliente, int? detalleOperacionId)
        {
            try
            {
                if (cliente != null)
                {
                    using (var context = new ComicsDbContext())
                    {
                        // Buscar el detalle_operacion (DO)
                        var doEncontrada = context.DetalleOperaciones
                            .FirstOrDefault(x =>
                                x.DetalleOperacionId == detalleOperacionId
                        );

                        if (doEncontrada != null)
                        {
                            // Verificar que el DO encontrado no lo tenga
                            // previamente asignado ningún otro cliente para
                            // respetar 1:M
                            bool mismoDo = context.Clientes
                            .Include(c => c.DetalleOperaciones)
                            .Any(c => c.DetalleOperaciones
                                .Any(d => d.DetalleOperacionId == 
                                          doEncontrada.DetalleOperacionId)
                            );

                            // Si no existe ningún otro cliente con el mismo DO
                            if (!mismoDo)
                            {
                                // Buscar el cliente a modificar
                                var clienteEncontrado = context.Clientes
                                    .Include(c => c.DetalleOperaciones)
                                    .FirstOrDefault(x =>
                                        x.ClienteId == cliente.ClienteId
                                    );

                                // Añadirle al cliente el DO
                                if (clienteEncontrado != null)
                                {
                                    doEncontrada.Clientes.Add(clienteEncontrado);
                                    context.SaveChanges();
                                    //return "";
                                    return 0;
                                }
                                else
                                {
                                    //return "Cliente no encontrado";
                                    return 1;
                                }
                            }
                            else
                            {
                                //return "Estos detalles pertenecen a otro cliente";
                                return 2;
                            }
                        }
                        else
                        {
                            //return "No ha sido posible encontrar los detalles de la operación";
                            return 3;
                        }
                    }
                }
                else
                {
                    //return "Es necesario un cliente para realizar la inserción";
                    return 4;
                }
            }
            catch (Exception e)
            {
                //return "Error sin determinar: " + e.ToString();
                return 5;
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
