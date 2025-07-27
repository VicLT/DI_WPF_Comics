using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Tipo_Operacion")]
public partial class TipoOperacion
{
    [Key]
    [Column("Tipo_OperacionID")]
    public int TipoOperacionId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [InverseProperty("TipoOperacion")]
    public virtual ICollection<Operacion> Operaciones { get; set; } = new List<Operacion>();

    public TipoOperacion(int tipoOperacionId, string? descripcion)
    {
        TipoOperacionId = tipoOperacionId;
        Descripcion = descripcion;
    }

    public TipoOperacion(int tipoOperacionId)
    {
        TipoOperacionId = tipoOperacionId;
    }

    public override string ToString()
    {
        return "Tipo operacion ID: " + TipoOperacionId + " # "
            + "Descripcion: " + (Descripcion ?? "-") + " # "
            + "Operaciones: " + Operaciones.Count;
    }
}
