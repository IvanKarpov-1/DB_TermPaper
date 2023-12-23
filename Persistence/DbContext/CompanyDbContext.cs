using Microsoft.EntityFrameworkCore;
using Persistence.DbModels;

namespace Persistence.DbContext;

public partial class CompanyDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<Factory> Factories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RepairRequest> RepairRequests { get; set; }

    public virtual DbSet<RepairRequestStatus> RepairRequestStatuses { get; set; }

    public DbSet<TEntity> GetEntityDbSet<TEntity>() where TEntity : class => Set<TEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("address_pkey");

            entity.ToTable("address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(70)
                .HasColumnName("address_line1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(70)
                .HasColumnName("address_line2");
            entity.Property(e => e.City)
                .HasMaxLength(35)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(2)
                .HasColumnName("country");
            entity.Property(e => e.PostCode)
                .HasMaxLength(16)
                .HasColumnName("post_code");
            entity.Property(e => e.Region)
                .HasMaxLength(35)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Email, "customer_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "customer_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(16)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.AddressId }).HasName("customer_address_pkey");

            entity.ToTable("customer_address");

            entity.HasIndex(e => e.AddressId, "customer_address_address_id_key").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");

            entity.HasOne(d => d.Address).WithOne(p => p.CustomerAddress)
                .HasForeignKey<CustomerAddress>(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_address_address_id_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("customer_address_customer_id_fkey");
        });

        modelBuilder.Entity<Factory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("factory_pkey");

            entity.ToTable("factory");

            entity.HasIndex(e => e.AddressId, "factory_address_id_key").IsUnique();

            entity.HasIndex(e => e.Email, "factory_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "factory_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(16)
                .HasColumnName("phone");

            entity.HasOne(d => d.Address).WithOne(p => p.Factory)
                .HasForeignKey<Factory>(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factory_address_id_fkey");

            entity.HasMany(d => d.Products).WithMany(p => p.Factories)
                .UsingEntity<Dictionary<string, object>>(
                    "FactoryProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("factory_product_product_id_fkey"),
                    l => l.HasOne<Factory>().WithMany()
                        .HasForeignKey("FactoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("factory_product_factory_id_fkey"),
                    j =>
                    {
                        j.HasKey("FactoryId", "ProductId").HasName("factory_product_pkey");
                        j.ToTable("factory_product");
                        j.IndexerProperty<int>("FactoryId").HasColumnName("factory_id");
                        j.IndexerProperty<int>("ProductId").HasColumnName("product_id");
                    });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pkey");

            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");

            entity.HasOne(d => d.CustomerAddress).WithMany(p => p.Orders)
                .HasForeignKey(d => new { d.CustomerId, d.AddressId })
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("order_customer_address_id_fkey");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_detail_pkey");

            entity.ToTable("order_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(9, 2)
                .HasColumnName("total_price");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_detail_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("order_detail_product_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Model)
                .HasMaxLength(20)
                .HasColumnName("model");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.NumberPerYear)
                .HasDefaultValue(0)
                .HasColumnName("number_per_year");
            entity.Property(e => e.SellingPrice)
                .HasPrecision(9, 2)
                .HasColumnName("selling_price");
            entity.Property(e => e.TechSpecs).HasColumnName("tech_specs");
            entity.Property(e => e.YearOfLaunch)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("year_of_launch");
        });

        modelBuilder.Entity<RepairRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_request_pkey");

            entity.ToTable("repair_request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.RequestDate).HasColumnName("request_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.RepairRequests)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("repair_request_customer_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.RepairRequests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("repair_request_status_id_fkey");
        });

        modelBuilder.Entity<RepairRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_request_status_pkey");

            entity.ToTable("repair_request_status");

            entity.HasIndex(e => e.StatusName, "repair_request_status_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(20)
                .HasColumnName("status_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
