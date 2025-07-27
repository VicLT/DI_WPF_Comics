using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

[Table("Stock_Comic")]
public partial class StockComic : IDisposable
{
    private bool disposedValue;

    [Key]
    [Column("Stock_ComicID")]
    public int StockComicId { get; set; }

    [Column("ComicID")]
    public int ComicId { get; set; }

    [Column("LocalID")]
    public int LocalId { get; set; }

    // Columna 'cantidad' añadida manualmente
    [Column("Cantidad")]
    public int? Cantidad { get; set; }

    [ForeignKey("ComicId")]
    [InverseProperty("StockComics")]
    public virtual Comic Comic { get; set; } = null!;

    [ForeignKey("LocalId")]
    [InverseProperty("StockComics")]
    public virtual Local Local { get; set; } = null!;

    public StockComic(/*int stockComicId,*/ int comicId, int localId,
        int? cantidad)
    {
        //StockComicId = stockComicId;
        ComicId = comicId;
        LocalId = localId;
        Cantidad = cantidad;
    }

    public override string ToString()
    {
        return "Stock comic ID: " + StockComicId + " # "
            + "Comic ID: " + ComicId + " # "
            + "Local ID: " + LocalId + " # "
            + "Cantidad: " + (Cantidad.ToString() ?? "-") + " # "
            + "Comic: " + (Comic != null ? Comic.ToString() + " # " : " - ")
            + "Local: " + (Local != null ? Local.ToString() + " # " : " - ");
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
