using System;
using System.Collections.Generic;
using DatosLoteria.Modelos;
using DatosLoteria.Static;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Data;

public partial class LoteriaContext : DbContext
{
    public LoteriaContext()
    {
    }

    public LoteriaContext(DbContextOptions<LoteriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    public virtual DbSet<Sorteo> Sorteos { get; set; }

    public virtual DbSet<VistaDeuda> VistaDeudas { get; set; }

    public virtual DbSet<VistaPago> VistaPagos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(StaticLoteria.CadenaConexion());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasOne(d => d.FkSocioNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pagos_Socios");

            entity.HasOne(d => d.FkSorteoNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pagos_Sorteos");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => new { e.FkSorteo, e.Inicio }).HasName("PK_Series_1");

            entity.HasOne(d => d.FkSocioNavigation).WithMany(p => p.Series)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Series_Socios");

            entity.HasOne(d => d.FkSorteoNavigation).WithMany(p => p.Series)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Series_Sorteos");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.HasKey(e => e.IdSocio).HasName("PK_Socios_1");

            entity.Property(e => e.Apellidos).IsFixedLength();
            entity.Property(e => e.CodigoPostal)
                .HasDefaultValue("46600")
                .IsFixedLength();
            entity.Property(e => e.Dni).IsFixedLength();
            entity.Property(e => e.Domicilio).IsFixedLength();
            entity.Property(e => e.Letra).IsFixedLength();
            entity.Property(e => e.Nombre).IsFixedLength();
            entity.Property(e => e.NombreCompleto).HasComputedColumnSql("((Trim([Apellidos])+', ')+Trim([Nombre]))", false);
            entity.Property(e => e.Poblacion)
                .HasDefaultValue("Alzira")
                .IsFixedLength();
        });

        modelBuilder.Entity<Sorteo>(entity =>
        {
            entity.HasKey(e => e.IdSorteo).HasName("PK_Sorteos_1");

            entity.Property(e => e.Numero).IsFixedLength();
            entity.Property(e => e.Precio).HasDefaultValue(500m);
        });

        modelBuilder.Entity<VistaDeuda>(entity =>
        {
            entity.ToView("VistaDeudas");
        });

        modelBuilder.Entity<VistaPago>(entity =>
        {
            entity.ToView("VistaPagos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
