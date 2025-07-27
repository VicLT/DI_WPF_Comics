using Microsoft.EntityFrameworkCore;

///<author>VICTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.Models;

public partial class ComicsDbContext : DbContext
{
    public ComicsDbContext()
    {
    }

    public ComicsDbContext(DbContextOptions<ComicsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autores { get; set; }

    public virtual DbSet<ClienteVlt> Clientes { get; set; }

    public virtual DbSet<Comic> Comics { get; set; }

    public virtual DbSet<DetalleOperacion> DetalleOperaciones { get; set; }

    public virtual DbSet<Editorial> Editoriales { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Local> Locales { get; set; }

    public virtual DbSet<MedioDePago> MediosDePago { get; set; }

    public virtual DbSet<Operacion> Operaciones { get; set; }

    public virtual DbSet<StockComic> StockComics { get; set; }

    public virtual DbSet<TipoOperacion> TiposOperacion { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Comics");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.AutorId).HasName("PK__Autor__F58AE909D7162F75");
        });

        modelBuilder.Entity<ClienteVlt>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente___71ABD0A77955C3D5");

            entity.HasMany(d => d.DetalleOperaciones).WithMany(p => p.Clientes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClienteDetalleOperacionVlt",
                    r => r.HasOne<DetalleOperacion>().WithMany()
                        .HasForeignKey("DetalleOperacionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Cliente_D__Detal__5DCAEF64"),
                    l => l.HasOne<ClienteVlt>().WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Cliente_D__Clien__5CD6CB2B"),
                    j =>
                    {
                        j.HasKey("ClienteId", "DetalleOperacionId").HasName("PK__Cliente___7C08759048B1115E");
                        j.ToTable("Cliente_Detalle_Operacion_VLT");
                        j.IndexerProperty<int>("ClienteId").HasColumnName("ClienteID");
                        j.IndexerProperty<int>("DetalleOperacionId").HasColumnName("Detalle_OperacionID");
                    });
        });

        modelBuilder.Entity<Comic>(entity =>
        {
            entity.HasKey(e => e.ComicId).HasName("PK__Comic__B8F0904E8A3A6A41");

            entity.HasOne(d => d.Autor).WithMany(p => p.Comics).HasConstraintName("FK__Comic__AutorID__286302EC");

            entity.HasOne(d => d.Editorial).WithMany(p => p.Comics).HasConstraintName("FK__Comic__Editorial__29572725");
        });

        modelBuilder.Entity<DetalleOperacion>(entity =>
        {
            entity.HasKey(e => e.DetalleOperacionId).HasName("PK__Detalle___DA3A53712B71632B");

            entity.HasOne(d => d.Comic).WithMany(p => p.DetalleOperaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Detalle_O__Comic__4316F928");

            entity.HasOne(d => d.Operacion).WithMany(p => p.DetalleOperaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Detalle_O__Opera__4222D4EF");
        });

        modelBuilder.Entity<Editorial>(entity =>
        {
            entity.HasKey(e => e.EditorialId).HasName("PK__Editoria__D54C828EEF5736FC");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__958BE6F037E7C2E3");

            entity.HasMany(d => d.Locales).WithMany(p => p.Empleados)
                .UsingEntity<Dictionary<string, object>>(
                    "LocalEmpleado",
                    r => r.HasOne<Local>().WithMany()
                        .HasForeignKey("LocalId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Local_Emp__Local__30F848ED"),
                    l => l.HasOne<Empleado>().WithMany()
                        .HasForeignKey("EmpleadoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Local_Emp__Emple__300424B4"),
                    j =>
                    {
                        j.HasKey("EmpleadoId", "LocalId").HasName("PK__Local_Em__3112D36D14020CFE");
                        j.ToTable("Local_Empleado");
                        j.IndexerProperty<int>("EmpleadoId").HasColumnName("EmpleadoID");
                        j.IndexerProperty<int>("LocalId").HasColumnName("LocalID");
                    });
        });

        modelBuilder.Entity<Local>(entity =>
        {
            entity.HasKey(e => e.LocalId).HasName("PK__Local__499359DB2FD37C88");
        });

        modelBuilder.Entity<MedioDePago>(entity =>
        {
            entity.HasKey(e => e.MedioDePagoId).HasName("PK__Medio_De__BBFD276345FA8C6B");
        });

        modelBuilder.Entity<Operacion>(entity =>
        {
            entity.HasKey(e => e.OperacionId).HasName("PK__Operacio__8A668AF7E6681174");

            entity.HasOne(d => d.Comic).WithMany(p => p.Operaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Operacion__Comic__3D5E1FD2");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Operaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Operacion__Emple__3F466844");

            entity.HasOne(d => d.Local).WithMany(p => p.Operaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Operacion__Local__3E52440B");

            entity.HasOne(d => d.MedioDePago).WithMany(p => p.Operaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Operacion__Medio__3B75D760");

            entity.HasOne(d => d.TipoOperacion).WithMany(p => p.Operaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Operacion__Tipo___3C69FB99");
        });

        modelBuilder.Entity<StockComic>(entity =>
        {
            entity.HasKey(e => e.StockComicId).HasName("PK__Stock_Co__3A77E09345362F02");

            entity.HasOne(d => d.Comic).WithMany(p => p.StockComics)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock_Com__Comic__34C8D9D1");

            entity.HasOne(d => d.Local).WithMany(p => p.StockComics)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock_Com__Local__33D4B598");
        });

        modelBuilder.Entity<TipoOperacion>(entity =>
        {
            entity.HasKey(e => e.TipoOperacionId).HasName("PK__Tipo_Ope__F75BE529F01C44F8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
