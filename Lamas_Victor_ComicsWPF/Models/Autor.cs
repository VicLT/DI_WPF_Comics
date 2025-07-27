using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Autor")]
public partial class Autor : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("AutorID")]
    public int AutorId { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Apellido { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Pais { get; set; }

    [InverseProperty("Autor")]
    public virtual ICollection<Comic> Comics { get; set; } = new List<Comic>();

    public string NombreCompleto
    {
        get
        {
            return Nombre + " " + Apellido;
        }
    }

    // ID comentado porque la BD gestiona automáticamente con Identity_Insert ON
    public Autor(/*int autorId,*/ string? nombre, string? apellido, string? pais)
    {
        //AutorId = autorId;
        Nombre = nombre;
        Apellido = apellido;
        Pais = pais;
    }

    public Autor(int autorId)
    {
        AutorId = autorId;
    }

    public override string ToString()
    {
        return "AutorID: " + AutorId + " # "
            + "Nombre: " + (Nombre ?? "-") + " # "
            + "Apellido: " + (Apellido ?? "-") + " # "
            + "País: " + (Pais ?? "-") + " # "
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
