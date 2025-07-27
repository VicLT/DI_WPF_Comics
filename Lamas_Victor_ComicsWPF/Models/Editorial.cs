using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Editorial")]
public partial class Editorial : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("EditorialID")]
    public int EditorialId { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [InverseProperty("Editorial")]
    public virtual ICollection<Comic> Comics { get; set; } = new List<Comic>();

    public Editorial(int editorialId, string? nombre)
    {
        EditorialId = editorialId;
        Nombre = nombre;
    }

    public Editorial(int editorialId)
    {
        EditorialId = editorialId;
    }

    public override string ToString()
    {
        return "Editorial ID: " + EditorialId + " # "
            + "Nombre: " + (Nombre ?? "-") + " # "
            + "Comics: " + Comics.Count;
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