using System;
using System.Collections.Generic;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductDataContexts : DbContext
{
    public ProductDataContexts()
    {
    }

    public ProductDataContexts(DbContextOptions<ProductDataContexts> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoryEntity> CategoryEntities { get; set; }

    public virtual DbSet<ManufactureEntity> ManufactureEntities { get; set; }

    public virtual DbSet<ProductEntity> ProductEntities { get; set; }

    public virtual DbSet<ProductPriceEntity> ProductPriceEntities { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=192.168.50.2;Initial Catalog=productcatalog_db_v1;Persist Security Info=True;User ID=evettainen;Password=Emil2024!;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07147682B1");

            entity.ToTable("CategoryEntity");

            entity.Property(e => e.CategoryName).HasMaxLength(50);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK__CategoryE__Paren__55F4C372");

            entity.HasMany(d => d.ArticleNumbers).WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategoryEntity",
                    r => r.HasOne<ProductEntity>().WithMany()
                        .HasForeignKey("ArticleNumber")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductCa__Artic__59C55456"),
                    l => l.HasOne<CategoryEntity>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ProductCa__Categ__58D1301D"),
                    j =>
                    {
                        j.HasKey("CategoryId", "ArticleNumber").HasName("PK__ProductC__3AC0AB1F6EA5AEB1");
                        j.ToTable("ProductCategoryEntity");
                        j.IndexerProperty<string>("ArticleNumber").HasMaxLength(200);
                    });
        });

        modelBuilder.Entity<ManufactureEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC0717754BD1");

            entity.ToTable("ManufactureEntity");

            entity.HasIndex(e => e.Manufacturers, "UQ__Manufact__D9127BD3737E84D0").IsUnique();

            entity.Property(e => e.Manufacturers).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductE__3C99114379ED61BE");

            entity.ToTable("ProductEntity");

            entity.Property(e => e.ArticleNumber).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Manufacture).WithMany(p => p.ProductEntities)
                .HasForeignKey(d => d.ManufactureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductEn__Manuf__503BEA1C");
        });

        modelBuilder.Entity<ProductPriceEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__ProductP__3C9911431AA86138");

            entity.ToTable("ProductPriceEntity");

            entity.Property(e => e.ArticleNumber).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.ProductPriceEntity)
                .HasForeignKey<ProductPriceEntity>(d => d.ArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PPE_ProductEntity_ArticleNumber");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
