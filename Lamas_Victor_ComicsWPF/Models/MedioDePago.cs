using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Medio_De_Pago")]
public partial class MedioDePago : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("Medio_De_PagoID")]
    public int MedioDePagoId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("Nombre_Corto")]
    [StringLength(25)]
    [Unicode(false)]
    public string? NombreCorto { get; set; }

    [InverseProperty("MedioDePago")]
    public virtual ICollection<Operacion> Operaciones { get; set; } = new List<Operacion>();

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public MedioDePago(/*int medioDePagoId,*/ string? descripcion,
        string? nombreCorto)
    {
        //MedioDePagoId = medioDePagoId;
        Descripcion = descripcion;
        NombreCorto = nombreCorto;
    }

    public MedioDePago(int medioDePagoId)
    {
        MedioDePagoId = medioDePagoId;
    }

    public override string ToString()
    {
        return "Medio de pago ID: " + MedioDePagoId + " # "
            + "Descripcion: " + (Descripcion ?? "-") + " # "
            + "Nombre corto: " + (NombreCorto ?? "-") + " # "
            + "Operaciones: " + Operaciones.Count;
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
