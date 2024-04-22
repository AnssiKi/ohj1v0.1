using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ohj1v0._1.Models;

public partial class VnContext : DbContext
{
    public VnContext()
    {
    }

    public VnContext(DbContextOptions<VnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alue> Alues { get; set; }

    public virtual DbSet<Asiaka> Asiakas { get; set; }

    public virtual DbSet<Lasku> Laskus { get; set; }

    public virtual DbSet<Mokki> Mokkis { get; set; }

    public virtual DbSet<Palvelu> Palvelus { get; set; }

    public virtual DbSet<Posti> Postis { get; set; }

    public virtual DbSet<Varau> Varaus { get; set; }

    public virtual DbSet<VarauksenPalvelut> VarauksenPalveluts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3307;database=vn;user=root;password=Ruutti", ServerVersion.Parse("11.3.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_bin")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Alue>(entity =>
        {
            entity.HasKey(e => e.AlueId).HasName("PRIMARY");

            entity.ToTable("alue");

            entity.HasIndex(e => e.Nimi, "alue_nimi_index");

            entity.Property(e => e.AlueId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("alue_id");
            entity.Property(e => e.Nimi)
                .HasMaxLength(40)
                .HasColumnName("nimi");
        });

        modelBuilder.Entity<Asiaka>(entity =>
        {
            entity.HasKey(e => e.AsiakasId).HasName("PRIMARY");

            entity.ToTable("asiakas");

            entity.HasIndex(e => e.Etunimi, "asiakas_enimi_idx");

            entity.HasIndex(e => e.Sukunimi, "asiakas_snimi_idx");

            entity.HasIndex(e => e.Postinro, "fk_as_posti1_idx");

            entity.Property(e => e.AsiakasId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("asiakas_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Etunimi)
                .HasMaxLength(20)
                .HasColumnName("etunimi");
            entity.Property(e => e.Lahiosoite)
                .HasMaxLength(40)
                .HasColumnName("lahiosoite");
            entity.Property(e => e.Postinro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("postinro");
            entity.Property(e => e.Puhelinnro)
                .HasMaxLength(15)
                .HasColumnName("puhelinnro");
            entity.Property(e => e.Sukunimi)
                .HasMaxLength(40)
                .HasColumnName("sukunimi");

            entity.HasOne(d => d.PostinroNavigation).WithMany(p => p.Asiakas)
                .HasForeignKey(d => d.Postinro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_asiakas_posti");
        });

        modelBuilder.Entity<Lasku>(entity =>
        {
            entity.HasKey(e => e.LaskuId).HasName("PRIMARY");

            entity.ToTable("lasku");

            entity.HasIndex(e => e.VarausId, "lasku_ibfk_1");

            entity.Property(e => e.LaskuId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("lasku_id");
            entity.Property(e => e.Alv)
                .HasColumnType("double(8,2)")
                .HasColumnName("alv");
            entity.Property(e => e.Maksettu)
                .HasColumnType("tinyint(4)")
                .HasColumnName("maksettu");
            entity.Property(e => e.Summa)
                .HasColumnType("double(8,2)")
                .HasColumnName("summa");
            entity.Property(e => e.VarausId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("varaus_id");

            entity.HasOne(d => d.Varaus).WithMany(p => p.Laskus)
                .HasForeignKey(d => d.VarausId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lasku_ibfk_1");
        });

        modelBuilder.Entity<Mokki>(entity =>
        {
            entity.HasKey(e => e.MokkiId).HasName("PRIMARY");

            entity.ToTable("mokki");

            entity.HasIndex(e => e.AlueId, "fk_mokki_alue_idx");

            entity.HasIndex(e => e.Postinro, "fk_mokki_posti_idx");

            entity.Property(e => e.MokkiId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("mokki_id");
            entity.Property(e => e.AlueId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("alue_id");
            entity.Property(e => e.Henkilomaara)
                .HasColumnType("int(11)")
                .HasColumnName("henkilomaara");
            entity.Property(e => e.Hinta)
                .HasColumnType("double(8,2)")
                .HasColumnName("hinta");
            entity.Property(e => e.Katuosoite)
                .HasMaxLength(45)
                .HasColumnName("katuosoite");
            entity.Property(e => e.Kuvaus)
                .HasMaxLength(150)
                .HasColumnName("kuvaus");
            entity.Property(e => e.Mokkinimi)
                .HasMaxLength(45)
                .HasColumnName("mokkinimi");
            entity.Property(e => e.Postinro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("postinro");
            entity.Property(e => e.Varustelu)
                .HasMaxLength(100)
                .HasColumnName("varustelu");

            entity.HasOne(d => d.Alue).WithMany(p => p.Mokkis)
                .HasForeignKey(d => d.AlueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mokki_alue");

            entity.HasOne(d => d.PostinroNavigation).WithMany(p => p.Mokkis)
                .HasForeignKey(d => d.Postinro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mokki_posti");
        });

        modelBuilder.Entity<Palvelu>(entity =>
        {
            entity.HasKey(e => e.PalveluId).HasName("PRIMARY");

            entity.ToTable("palvelu");

            entity.HasIndex(e => e.Nimi, "Palvelu_nimi_index");

            entity.HasIndex(e => e.AlueId, "palv_toimip_id_ind");

            entity.Property(e => e.PalveluId)
                .ValueGeneratedNever()
                .HasColumnType("int(10) unsigned")
                .HasColumnName("palvelu_id");
            entity.Property(e => e.AlueId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("alue_id");
            entity.Property(e => e.Alv)
                .HasColumnType("double(8,2)")
                .HasColumnName("alv");
            entity.Property(e => e.Hinta)
                .HasColumnType("double(8,2)")
                .HasColumnName("hinta");
            entity.Property(e => e.Kuvaus)
                .HasMaxLength(255)
                .HasColumnName("kuvaus");
            entity.Property(e => e.Nimi)
                .HasMaxLength(40)
                .HasColumnName("nimi");

            entity.HasOne(d => d.Alue).WithMany(p => p.Palvelus)
                .HasForeignKey(d => d.AlueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("palvelu_ibfk_1");
        });

        modelBuilder.Entity<Posti>(entity =>
        {
            entity.HasKey(e => e.Postinro).HasName("PRIMARY");

            entity.ToTable("posti");

            entity.Property(e => e.Postinro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("postinro");
            entity.Property(e => e.Toimipaikka)
                .HasMaxLength(45)
                .HasColumnName("toimipaikka");
        });

        modelBuilder.Entity<Varau>(entity =>
        {
            entity.HasKey(e => e.VarausId).HasName("PRIMARY");

            entity.ToTable("varaus");

            entity.HasIndex(e => e.MokkiId, "fk_var_mok_idx");

            entity.HasIndex(e => e.AsiakasId, "varaus_as_id_index");

            entity.Property(e => e.VarausId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("varaus_id");
            entity.Property(e => e.AsiakasId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("asiakas_id");
            entity.Property(e => e.MokkiId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("mokki_id");
            entity.Property(e => e.VahvistusPvm)
                .HasColumnType("datetime")
                .HasColumnName("vahvistus_pvm");
            entity.Property(e => e.VarattuAlkupvm)
                .HasColumnType("datetime")
                .HasColumnName("varattu_alkupvm");
            entity.Property(e => e.VarattuLoppupvm)
                .HasColumnType("datetime")
                .HasColumnName("varattu_loppupvm");
            entity.Property(e => e.VarattuPvm)
                .HasColumnType("datetime")
                .HasColumnName("varattu_pvm");

            entity.HasOne(d => d.Asiakas).WithMany(p => p.Varaus)
                .HasForeignKey(d => d.AsiakasId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("varaus_ibfk");

            entity.HasOne(d => d.Mokki).WithMany(p => p.Varaus)
                .HasForeignKey(d => d.MokkiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_varaus_mokki");
        });

        modelBuilder.Entity<VarauksenPalvelut>(entity =>
        {
            entity.HasKey(e => new { e.PalveluId, e.VarausId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("varauksen_palvelut");

            entity.HasIndex(e => e.PalveluId, "vp_palvelu_id_index");

            entity.HasIndex(e => e.VarausId, "vp_varaus_id_index");

            entity.Property(e => e.PalveluId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("palvelu_id");
            entity.Property(e => e.VarausId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("varaus_id");
            entity.Property(e => e.Lkm)
                .HasColumnType("int(11)")
                .HasColumnName("lkm");

            entity.HasOne(d => d.Palvelu).WithMany(p => p.VarauksenPalveluts)
                .HasForeignKey(d => d.PalveluId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_palvelu");

            entity.HasOne(d => d.Varaus).WithMany(p => p.VarauksenPalveluts)
                .HasForeignKey(d => d.VarausId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_varaus");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
