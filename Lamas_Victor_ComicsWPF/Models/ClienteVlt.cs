using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Cliente_VLT")]
public partial class ClienteVlt : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("ClienteID")]
    public int ClienteId { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Apellido { get; set; }

    [Column("NIF")]
    [StringLength(10)]
    [Unicode(false)]
    public string Nif { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Direccion { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("Clientes")]
    public virtual ICollection<DetalleOperacion> DetalleOperaciones { get; set; } = new List<DetalleOperacion>();

    public string NombreCompleto
    {
        get
        {
            return Nombre + " " + Apellido;
        }
    }

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public ClienteVlt(/*int clienteId,*/ string? nombre, string? apellido,
        string nif, string? direccion)
    {
        //ClienteId = clienteId;
        Nombre = nombre;
        Apellido = apellido;
        Nif = nif;
        Direccion = direccion;
    }

    public ClienteVlt(int clienteId, string nif)
    {
        ClienteId = clienteId;
        Nif = nif;
    }

    public override string ToString()
    {
        return "Cliente ID: " + ClienteId + " # "
            + "Nombre: " + (Nombre ?? "-") + " # "
            + "Apellido: " + (Apellido ?? "-") + " # "
            + "NIF: " + Nif + " # "
            + "Direccion: " + (Direccion ?? "-") + " # "
            + "Detalle operaciones: " + DetalleOperaciones.Count;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
            }
            disposedValue = true;
        }
    }
}