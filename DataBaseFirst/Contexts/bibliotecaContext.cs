using System;
using System.Collections.Generic;
using DataBaseFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBaseFirst.Contexts;

public partial class bibliotecaContext : DbContext
{
    public bibliotecaContext()
    {
    }

    public bibliotecaContext(DbContextOptions<bibliotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<categorias> categorias { get; set; }

    public virtual DbSet<libros> libros { get; set; }

    public virtual DbSet<prestamos> prestamos { get; set; }

    public virtual DbSet<t_personal> t_personals { get; set; }

    public virtual DbSet<usuarios> usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=sistemaBiblioteca;User ID=brian;Password=123456;Trusted_Connection=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<categorias>(entity =>
        {
            entity.HasKey(e => e.id_categoria).HasName("PK__categori__CD54BC5A45C914CF");

            entity.Property(e => e.descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.nombre_categoria)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<libros>(entity =>
        {
            entity.HasKey(e => e.id_libro).HasName("PK__libros__EC09C24E78E9F054");

            entity.Property(e => e.ISBN)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.autor)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.titulo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.id_categoriaNavigation).WithMany(p => p.libros)
                .HasForeignKey(d => d.id_categoria)
                .HasConstraintName("FK__libros__id_categ__66603565");
        });

        modelBuilder.Entity<prestamos>(entity =>
        {
            entity.HasKey(e => e.id_prestamo).HasName("PK__prestamo__5E87BE27B46DF502");

            entity.HasOne(d => d.id_libroNavigation).WithMany(p => p.prestamos)
                .HasForeignKey(d => d.id_libro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__prestamos__id_li__6A30C649");

            entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.prestamos)
                .HasForeignKey(d => d.id_usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__prestamos__id_us__693CA210");
        });

        modelBuilder.Entity<t_personal>(entity =>
        {
            entity.HasKey(e => e.id_personal).HasName("PK__t_person__418FB8081B4B91BD");

            entity.ToTable("t_personal");

            entity.Property(e => e.correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.password_hash)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<usuarios>(entity =>
        {
            entity.HasKey(e => e.id_usuario).HasName("PK__usuarios__4E3E04AD17427B52");

            entity.Property(e => e.correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
