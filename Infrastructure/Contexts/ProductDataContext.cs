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

    public virtual DbSet<ProductPriceEntity> ProductPriceEntities { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=192.168.50.2;Initial Catalog=productcatalog_db_v1;User ID=evettainen;Password=Emil2024!;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC073D54524D");

            entity.ToTable("CategoryEntity");

            entity.Property(e => e.CategoryName).HasMaxLength(50);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK__CategoryE__Paren__65370702");

            entity.HasMany(d => d.ArticleNumbers).WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategoryEntity",
                    r => r.HasOne<ProductEntity>().WithMany()
                        .HasForeignKey("ArticleNumber")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductCa__Artic__690797E6"),
                    l => l.HasOne<CategoryEntity>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductCa__Categ__681373AD"),
                    j =>
                    {
                        j.HasKey("CategoryId", "ArticleNumber").HasName("PK__ProductC__3AC0AB1F31D8A8E2");
                        j.ToTable("ProductCategoryEntity");
                        j.IndexerProperty<string>("ArticleNumber").HasMaxLength(200);
                    });
        });

        modelBuilder.Entity<ManufactureEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC07AB169657");

            entity.ToTable("ManufactureEntity");

            entity.HasIndex(e => e.Manufacturers, "UQ__Manufact__D9127BD3912EAA01").IsUnique();

            entity.Property(e => e.Manufacturers).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductE__3C9911430B1D0D74");

            entity.ToTable("ProductEntity");

            entity.Property(e => e.ArticleNumber).HasMaxLength(200);
            entity.Property(e => e.Ingress).HasMaxLength(450);
            entity.Property(e => e.ProductTitle).HasMaxLength(100);

            entity.HasOne(d => d.Manufacture).WithMany(p => p.ProductEntities)
                .HasForeignKey(d => d.ManufactureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductEn__Manuf__5F7E2DAC");
        });

        modelBuilder.Entity<ProductPriceEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductP__3C9911433F410D29");

            entity.ToTable("ProductPriceEntity");

            entity.Property(e => e.ArticleNumber).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.ProductPriceEntity)
                .HasForeignKey<ProductPriceEntity>(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductPr__Artic__625A9A57");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
