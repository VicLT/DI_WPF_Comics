using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Empleado")]
public partial class Empleado : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("EmpleadoID")]
    public int EmpleadoId { get; set; }

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

    [StringLength(64)]
    [Unicode(false)]
    public string? Password { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? Activo { get; set; }

    [Column("Fecha_alta", TypeName = "date")]
    public DateTime? FechaAlta { get; set; }

    [Column("Fecha_baja", TypeName = "date")]
    public DateTime? FechaBaja { get; set; }

    [Column("Fotografia", TypeName = "image")]
    public byte[]? Fotografia { get; set; }

    [InverseProperty("Empleado")]
    public virtual ICollection<Operacion> Operaciones { get; set; } = new List<Operacion>();

    [ForeignKey("EmpleadoId")]
    [InverseProperty("Empleados")]
    public virtual ICollection<Local> Locales { get; set; } = new List<Local>();

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public Empleado(/*int empleadoId,*/ string? nombre, string? apellido,
        string nif, string? direccion, string? password, string? email,
        string? activo, DateTime? fechaAlta, DateTime? fechaBaja,
        byte[]? fotografia)
    {
        //EmpleadoId = empleadoId;
        Nombre = nombre;
        Apellido = apellido;
        Nif = nif;
        Direccion = direccion;
        Password = password;
        Email = email;
        Activo = activo;
        FechaAlta = fechaAlta;
        FechaBaja = fechaBaja;
        Fotografia = fotografia;
    }

    public Empleado(int empleadoId, string nif)
    {
        EmpleadoId = empleadoId;
        Nif = nif;
    }

    public override string ToString()
    {
        return "Empleado ID: " + EmpleadoId + " # "
            + "Nombre: " + (Nombre ?? "-") + " # "
            + "Apellido: " + (Apellido ?? "-") + " # "
            + "NIF: " + Nif + " # "
            + "Direccion: " + (Direccion ?? "-") + " # "
            + "Password: " + (Password ?? "-") + " # "
            + "Email: " + (Email ?? "-") + " # "
            + "Activo: " + (Activo ?? "-") + " # "
            + "Fecha alta: " + (FechaAlta.ToString() ?? "-") + " # "
            + "Fecha baja: " + (FechaBaja.ToString() ?? "-") + " # "
            //+ "Fotografía: " + (Fotografia != null ? Fotografia.ToString() + " # " : " - ")
            + "Operaciones: " + Operaciones.Count + " # "
            + "Locales: " + Locales.Count;
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
