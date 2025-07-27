using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Comic")]
public partial class Comic : IComparable<Comic>, IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("ComicID")]
    public int ComicId { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("AutorID")]
    public int? AutorId { get; set; }

    [Column("EditorialID")]
    public int? EditorialId { get; set; }

    [Column("Precio_Compra", TypeName = "decimal(10, 2)")]
    public decimal? PrecioCompra { get; set; }

    [Column("Precio_Venta", TypeName = "decimal(10, 2)")]
    public decimal? PrecioVenta { get; set; }

    [ForeignKey("AutorId")]
    [InverseProperty("Comics")]
    public virtual Autor? Autor { get; set; }

    [InverseProperty("Comic")]
    public virtual ICollection<DetalleOperacion> DetalleOperaciones { get; set; }
        = new List<DetalleOperacion>();

    [ForeignKey("EditorialId")]
    [InverseProperty("Comics")]
    public virtual Editorial? Editorial { get; set; }

    [InverseProperty("Comic")]
    public virtual ICollection<Operacion> Operaciones { get; set; } = new List<Operacion>();

    [InverseProperty("Comic")]
    public virtual ICollection<StockComic> StockComics { get; set; } = new List<StockComic>();

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public Comic(/*int comicId,*/ string? nombre, int? autorId, int? editorialId,
        decimal? precioCompra, decimal? precioVenta)
    {
        //ComicId = comicId;
        Nombre = nombre;
        AutorId = autorId;
        EditorialId = editorialId;
        PrecioCompra = precioCompra;
        PrecioVenta = precioVenta;
    }

    public Comic(int comicId)
    {
        ComicId = comicId;
    }

    public Comic() { }

    public int CompareTo(Comic? otro)
    {
        if (otro == null)
        {
            return 1;
        }

        string thisEditorialNombre = Editorial?.Nombre ?? string.Empty;
        string otroEditorialNombre = otro.Editorial?.Nombre ?? string.Empty;

        int comparativa = thisEditorialNombre.CompareTo(otroEditorialNombre);
        if (comparativa == 0)
        {
            string thisComicNombre = Nombre ?? string.Empty;
            string otroComicNombre = otro.Nombre ?? string.Empty;
            comparativa = thisComicNombre.CompareTo(otroComicNombre);
        }
        return comparativa;
    }

    public override string ToString()
    {
        return "Comic ID: " + ComicId + " # "
            + "Nombre: " + (Nombre ?? "-") + " # "
            + "Autor ID: " + (AutorId.ToString() ?? "-") + " # "
            + "Editorial ID: " + (EditorialId.ToString() ?? "-") + " # "
            + "Precio compra: " + (PrecioCompra.ToString() ?? "-") + " # "
            + "Precio venta: " + (PrecioVenta.ToString() ?? "-") + " # "
            + "Autor: " + (Autor?.ToString() ?? "-") + " # "
            + "Detalle operaciones: " + DetalleOperaciones.Count + " # "
            + "Editorial: " + (Editorial?.ToString() ?? "-") + "#"
            + "Operaciones: " + Operaciones.Count + " # "
            + "Stock: " + StockComics.Count;
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