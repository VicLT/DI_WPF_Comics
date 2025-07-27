using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Operacion")]
public partial class Operacion : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("OperacionID")]
    public int OperacionId { get; set; }

    [Column("Medio_De_PagoID")]
    public int MedioDePagoId { get; set; }

    [Column("Tipo_OperacionID")]
    public int TipoOperacionId { get; set; }

    [Column("ComicID")]
    public int ComicId { get; set; }

    [Column("LocalID")]
    public int LocalId { get; set; }

    [Column("Fecha_Operacion", TypeName = "datetime")]
    public DateTime FechaOperacion { get; set; }

    [Column("EmpleadoID")]
    public int EmpleadoId { get; set; }

    [ForeignKey("ComicId")]
    [InverseProperty("Operaciones")]
    public virtual Comic Comic { get; set; } = null!;

    [InverseProperty("Operacion")]
    public virtual ICollection<DetalleOperacion> DetalleOperaciones { get; set; }
        = new List<DetalleOperacion>();

    [ForeignKey("EmpleadoId")]
    [InverseProperty("Operaciones")]
    public virtual Empleado Empleado { get; set; } = null!;

    [ForeignKey("LocalId")]
    [InverseProperty("Operaciones")]
    public virtual Local Local { get; set; } = null!;

    [ForeignKey("MedioDePagoId")]
    [InverseProperty("Operaciones")]
    public virtual MedioDePago MedioDePago { get; set; } = null!;

    [ForeignKey("TipoOperacionId")]
    [InverseProperty("Operaciones")]
    public virtual TipoOperacion TipoOperacion { get; set; } = null!;

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public Operacion(/*int operacionId,*/ int medioDePagoId, int tipoOperacionId,
        int comicId, int localId, DateTime fechaOperacion, int empleadoId)
    {
        //OperacionId = operacionId;
        MedioDePagoId = medioDePagoId;
        TipoOperacionId = tipoOperacionId;
        ComicId = comicId;
        LocalId = localId;
        FechaOperacion = fechaOperacion;
        EmpleadoId = empleadoId;
    }

    public override string ToString()
    {
        return "Operacion ID: " + OperacionId + " # "
            + "Medio de pago ID: " + MedioDePagoId + " # "
            + "Tipo operacion ID: " + TipoOperacionId + " # "
            + "Comic ID: " + ComicId + " # "
            + "Local ID: " + LocalId + " # "
            + "Fecha operacion: " + FechaOperacion + " # "
            + "Empleado ID: " + EmpleadoId + " # "
            + "Comic: " + (Comic != null ? Comic.ToString() + " # " : " - ")
            + "Detalle operaciones: " + DetalleOperaciones.Count + " # "
            + "Empleado: "
            + (Empleado != null ? Empleado.ToString() + " # " : " - ")
            + "Local: " + (Local != null ? Local.ToString() + " # " : " - ")
            + "Medio de pago: "
            + (MedioDePago != null ? MedioDePago.ToString() + " # " : " - ")
            + "Tipo operacion: "
            + (TipoOperacion != null ? TipoOperacion.ToString() : " - ");
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
