using System;
using System.Collections.Generic;
using DatosLoteria.Modelos;
using DatosLoteria.Static;
using Microsoft.EntityFrameworkCore;

namespace DatosLoteria.Contexto;

public partial class LoteriaContext : DbContext
{
    public LoteriaContext()
    {
    }

    public LoteriaContext(DbContextOptions<LoteriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Deuda> Deudas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Serie> Series { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    public virtual DbSet<Sorteo> Sorteos { get; set; }

    public virtual DbSet<VistaCompra> VistaCompras { get; set; }

    public virtual DbSet<VistaPago> VistaPagos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(StaticLoteria.CadenaConexion());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deuda>(entity =>
        {
            entity.ToView("Deudas");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasOne(d => d.FkSocioNavigation).WithMany(p => p.Pagos).HasConstraintName("FK_Pago_Socio");

            entity.HasOne(d => d.FkSorteoNavigation).WithMany(p => p.Pagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Sorteo");
        });

        modelBuilder.Entity<Serie>(entity =>
        {
            entity.HasOne(d => d.FkSocioNavigation).WithMany(p => p.Series).HasConstraintName("FK_Serie_Socio");

            entity.HasOne(d => d.FkSorteoNavigation).WithMany(p => p.Series)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Serie_Sorteo");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.Property(e => e.Apellidos).IsFixedLength();
            entity.Property(e => e.Domicilio).IsFixedLength();
            entity.Property(e => e.Letra).IsFixedLength();
            entity.Property(e => e.Nombre).IsFixedLength();
            entity.Property(e => e.NombreCompleto).HasComputedColumnSql("((Trim([Apellidos])+', ')+Trim([Nombre]))", false);
            entity.Property(e => e.Poblacion).IsFixedLength();
        });

        modelBuilder.Entity<Sorteo>(entity =>
        {
            entity.Property(e => e.Precio).HasDefaultValue(5m);
        });

        modelBuilder.Entity<VistaCompra>(entity =>
        {
            entity.ToView("Vista_Compras");
        });

        modelBuilder.Entity<VistaPago>(entity =>
        {
            entity.ToView("Vista_Pagos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
