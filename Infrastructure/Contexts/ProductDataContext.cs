using System;
using System.Collections.Generic;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductDataContext : DbContext
{
    public ProductDataContext()
    {
    }

    public ProductDataContext(DbContextOptions<ProductDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoryEntity> CategoryEntities { get; set; }

    public virtual DbSet<ManufactureEntity> ManufactureEntities { get; set; }

    public virtual DbSet<ProductEntity> ProductEntities { get; set; }

    public virtual DbSet<ProductInfoEntity> ProductInfoEntities { get; set; }

    public virtual DbSet<ProductPriceEntity> ProductPriceEntities { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=192.168.50.2;Initial Catalog=productcatalog_db_v1;User ID=evettainen;Password=Emil2024!;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC078DD82942");

            entity.ToTable("CategoryEntity");

            entity.HasIndex(e => e.CategoryName, "UQ__Category__8517B2E0DC12540F").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<ManufactureEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC0788661B81");

            entity.ToTable("ManufactureEntity");

            entity.HasIndex(e => e.ManufactureName, "UQ__Manufact__00DD03CE64908379").IsUnique();

            entity.Property(e => e.ManufactureName).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductE__3C99114399799C68");

            entity.ToTable("ProductEntity");

            entity.Property(e => e.ArticleNumber).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.ProductEntities)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductEn__Categ__01D345B0");

            entity.HasOne(d => d.Manufacture).WithMany(p => p.ProductEntities)
                .HasForeignKey(d => d.ManufactureId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductEn__Manuf__00DF2177");
        });

        modelBuilder.Entity<ProductInfoEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductI__3C9911436C05471D");

            entity.ToTable("ProductInfoEntity");

            entity.Property(e => e.ArticleNumber).HasMaxLength(200);
            entity.Property(e => e.Ingress).HasMaxLength(450);
            entity.Property(e => e.ProductTitle).HasMaxLength(100);

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.ProductInfoEntity)
                .HasForeignKey<ProductInfoEntity>(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductIn__Artic__04AFB25B");
        });

        modelBuilder.Entity<ProductPriceEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductP__3C99114399D0962C");

            entity.ToTable("ProductPriceEntity");

            entity.Property(e => e.ArticleNumber).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.ProductPriceEntity)
                .HasForeignKey<ProductPriceEntity>(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductPr__Artic__078C1F06");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
