using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Local")]
public partial class Local : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("LocalID")]
    public int LocalId { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Direccion { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal Latitud { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal Longitud { get; set; }

    [InverseProperty("Local")]
    public virtual ICollection<Operacion> Operaciones { get; set; } =
        new List<Operacion>();

    [InverseProperty("Local")]
    public virtual ICollection<StockComic> StockComics { get; set; } =
        new List<StockComic>();

    [ForeignKey("LocalId")]
    [InverseProperty("Locales")]
    public virtual ICollection<Empleado> Empleados { get; set; } =
        new List<Empleado>();

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public Local(/*int localId,*/ string? nombre, string? direccion,
        decimal latitud, decimal longitud)
    {
        //LocalId = localId;
        Nombre = nombre;
        Direccion = direccion;
        Latitud = latitud;
        Longitud = longitud;
    }

    public Local(int localId, decimal latitud, decimal longitud)
    {
        LocalId = localId;
        Latitud = latitud;
        Longitud = longitud;
    }

    public override string ToString()
    {
        return "Local ID: " + LocalId + " # "
            + "Nombre: " + (Nombre ?? "-") + " # "
            + "Direccion: " + (Direccion ?? "-") + " # "
            + "Latitud: " + Latitud + " # "
            + "Longitud: " + Longitud + " # "
            + "Operaciones: " + Operaciones.Count + " # "
            + "Stock comics: " + StockComics.Count + " # "
            + "Empleados: " + Empleados.Count;
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
