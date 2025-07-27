using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Detalle_Operacion")]
public partial class DetalleOperacion : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("Detalle_OperacionID")]
    public int DetalleOperacionId { get; set; }

    [Column("OperacionID")]
    public int OperacionId { get; set; }

    [Column("ComicID")]
    public int ComicId { get; set; }

    public int? Cantidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Precio { get; set; }

    [Column(TypeName = "decimal(7, 4)")]
    public decimal? Descuento { get; set; }

    [ForeignKey("ComicId")]
    [InverseProperty("DetalleOperaciones")]
    public virtual Comic Comic { get; set; } = null!;

    [ForeignKey("OperacionId")]
    [InverseProperty("DetalleOperaciones")]
    public virtual Operacion Operacion { get; set; } = null!;

    [ForeignKey("DetalleOperacionId")]
    [InverseProperty("DetalleOperaciones")]
    public virtual ICollection<ClienteVlt> Clientes { get; set; } = new List<ClienteVlt>();

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public DetalleOperacion(/*int detalleOperacionId,*/ int operacionId,
        int comicId, int? cantidad, decimal? precio, decimal? descuento)
    {
        //DetalleOperacionId = detalleOperacionId;
        OperacionId = operacionId;
        ComicId = comicId;
        Cantidad = cantidad;
        Precio = precio;
        Descuento = descuento;
    }

    public DetalleOperacion(int detalleOperacion, int operacionId, int comicId)
    {
        DetalleOperacionId = detalleOperacion;
        OperacionId = operacionId;
        ComicId = comicId;
    }

    public override string ToString()
    {
        return "Detalle operacion ID: " + DetalleOperacionId + " # "
            + "Operacion ID: " + OperacionId + " # "
            + "Comic ID: " + ComicId + " # "
            + "Cantidad: " + (Cantidad.ToString() ?? "-") + " # "
            + "Precio: " + (Precio.ToString() ?? "-") + " # "
            + "Descuento: " + (Descuento.ToString() ?? "-") + " # "
            + "Cómic: " + (Comic != null ? Comic.ToString() + " # " : " - ")
            + "Operación: " + (Operacion != null ? Operacion.ToString() + " # " : " - ")
            + "Clientes: " + Clientes.Count;
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
