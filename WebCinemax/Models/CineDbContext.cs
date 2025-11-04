using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebCinemax.Models;

public partial class CineDbContext : DbContext
{
    public CineDbContext()
    {
    }

    public CineDbContext(DbContextOptions<CineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Butaca> Butacas { get; set; }

    public virtual DbSet<Funcion> Funciones { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<ReservaButaca> ReservaButacas { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    public virtual DbSet<Snack> Snacks { get; set; }

    public virtual DbSet<SnackReserva> SnackReservas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
  //      => optionsBuilder.UseSqlServer("Server=localhost;Database=Cinemaxdb;Trusted_Connection=True;TrustServerCertificate=True;");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Butaca>(entity =>
        {
            entity.HasKey(e => e.IdButaca).HasName("PK__Butacas__BD3D02A285977DEF");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Butacas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Butaca_Sala");
        });

        modelBuilder.Entity<Funcion>(entity =>
        {
            entity.HasKey(e => e.IdFuncion).HasName("PK__Funcione__A2FDB89ABE93D40A");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Funciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Funcion_Pelicula");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Funciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Funcion_Sala");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdInventario).HasName("PK__Inventar__8F145B0DAB6CDADA");

            entity.HasOne(d => d.IdSnackNavigation).WithOne(p => p.Inventario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Snack");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.IdPelicula).HasName("PK__Pelicula__9F5B678A7A0E81AD");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reservas__94D104C8B7673173");

            entity.Property(e => e.FechaReserva).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdFuncionNavigation).WithMany(p => p.Reservas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Funcion");
        });

        modelBuilder.Entity<ReservaButaca>(entity =>
        {
            entity.HasKey(e => e.IdReservaButaca).HasName("PK__ReservaB__2CD4A1C79B0C348A");

            entity.HasOne(d => d.IdButacaNavigation).WithMany(p => p.ReservaButacas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservaButaca_Butaca");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.ReservaButacas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservaButaca_Reserva");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.IdSala).HasName("PK__Salas__C4AEB19C1AEF717F");
        });

        modelBuilder.Entity<Snack>(entity =>
        {
            entity.HasKey(e => e.IdSnack).HasName("PK__Snacks__F684702AE4F6507A");
        });

        modelBuilder.Entity<SnackReserva>(entity =>
        {
            entity.HasKey(e => e.IdSnackReserva).HasName("PK__SnackRes__08D0D150A4E6B4C3");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.SnackReservas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SnackReserva_Reserva");

            entity.HasOne(d => d.IdSnackNavigation).WithMany(p => p.SnackReservas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SnackReserva_Snack");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__645723A690949ACA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
