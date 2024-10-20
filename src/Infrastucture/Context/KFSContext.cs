using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Infrastucture.Repository;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Context
{
    public class KFSContext : DbContext
    {
        public KFSContext()
        {

        }
        public KFSContext(DbContextOptions<KFSContext> options) : base(options)
        {

        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Consignment> Consignments { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<PaymentOrder> PaymentOrders { get; set; }
        public DbSet<PaymentConsignment> PaymentConsignments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //seed role
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = RoleConst.ADMIN_ID,
                    Name = RoleConst.ADMIN
                },
                new Role
                {
                    RoleId = RoleConst.MANAGER_ID,
                    Name = RoleConst.MANAGER
                },
                new Role
                {
                    RoleId = RoleConst.STAFF_ID,
                    Name = RoleConst.STAFF
                },
                new Role
                {
                    RoleId = RoleConst.CUSTOMER_ID,
                    Name = RoleConst.CUSTOMER
                },
                new Role
                {
                    RoleId = RoleConst.GUEST_ID,
                    Name = RoleConst.GUEST
                }
            );
            //seed permission
            modelBuilder.Entity<Permission>()
            .HasData(
                new Permission { Slug = PermissionSlug.MANAGE_USER, Name = "Manage User" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_USER, Name = "Manage Own User" },
                new Permission { Slug = PermissionSlug.MANAGE_PERMISSION, Name = "Manage Permission" },
                new Permission { Slug = PermissionSlug.MANAGE_ROLE, Name = "Manage Role" },
                new Permission { Slug = PermissionSlug.MANAGE_PRODUCT, Name = "Manage Product" },
                new Permission { Slug = PermissionSlug.MANAGE_ORDER, Name = "Manage Order" },
                new Permission { Slug = PermissionSlug.MANAGE_CATEGORY, Name = "Manage Category" },
                new Permission { Slug = PermissionSlug.MANAGE_FEEDBACK, Name = "Manage Feedback" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_FEEDBACK, Name = "Manage Own Feedback" }
            );
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j
                .HasData(
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_USER },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_OWN_USER },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_PERMISSION },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_ROLE },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_PRODUCT },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_ORDER },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_CATEGORY },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_FEEDBACK },
                    new { RolesRoleId = RoleConst.ADMIN_ID, PermissionsSlug = PermissionSlug.MANAGE_OWN_FEEDBACK })
);
            // Seed user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    FullName = "Truong Giang",
                    Email = "giangnnt260703@gmail.com",
                    RoleId = RoleConst.ADMIN_ID,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Phone = "0123456789",
                    Address = "HCM",
                    CreatedAt = DateTime.Parse("2024-10-11"),
                }
            );

            // Seed wallet separately and specify the foreign key value
            modelBuilder.Entity<Wallet>().HasData(
                new Wallet
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Point = 20000
                }
            );
            //seed category
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = Guid.Parse("5F18BF0C-7199-462C-B023-3CCF1FD9F806"), Name = "Thuần chủng nhập khẩu" },
                new Category { Id = Guid.Parse("3D4FC185-049D-4A96-851B-1D320E7DBBA8"), Name = "Lai F1" },
                new Category { Id = Guid.Parse("9A17DCF5-1426-45EE-A32E-C23EE5FE40D9"), Name = "Thuần Việt" }
            );
            //config product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.Gender).HasConversion(v => v.ToString(), v => v != null ? (GenderEnum)Enum.Parse(typeof(GenderEnum), v) : default)
                .HasColumnType("nvarchar(20)");
                entity.HasMany(p => p.Batches)
                .WithOne(b => b.Product)
                .OnDelete(DeleteBehavior.Restrict);
            });
            // seed batch
            modelBuilder.Entity<Batch>().HasData(
                new Batch
                {
                    Id = Guid.Parse("64141e04-9c9d-4bd1-94d2-84ee359f1e5b"),
                    Name = "Batch 1",
                    ProductId = Guid.Parse("2a9394e2-52b3-46d5-8a33-af4d6020e440"),
                    Quantity = 10,
                    Inventory = 10,
                    Price = 10000,
                },
                new Batch
                {
                    Id = Guid.Parse("eb5707d1-1b02-4ad5-8d6b-27a78d00f322"),
                    Name = "Batch 2",
                    ProductId = Guid.Parse("8657ed40-1b9d-44e2-800d-40bb1a20af98"),
                    Quantity = 10,
                    Inventory = 10,
                    Price = 20000,
                    IsForSell = true
                },
                new Batch
                {
                    Id = Guid.Parse("b6bfe977-ee7e-4e84-a65b-445c36d08d65"),
                    Name = "Batch 3",
                    ProductId = Guid.Parse("f3b3b3b4-1b9d-44e2-800d-40bb1a20af98"),
                    Quantity = 10,
                    Inventory = 10,
                    Price = 30000,
                    IsForSell = true
                }
            );
            //config payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.Status).HasConversion(v => v.ToString(), v => v != null ? (PaymentStatusEnum)Enum.Parse(typeof(PaymentStatusEnum), v) : default)
                .HasColumnType("nvarchar(20)");
                entity.Property(entity => entity.PaymentMethod).HasConversion(v => v.ToString(), v => v != null ? (PaymentMethodEnum)Enum.Parse(typeof(PaymentMethodEnum), v) : default)
                .HasColumnType("nvarchar(20)");
            });
            //config shipment
            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasOne(entity => entity.Order)
                .WithOne(entity => entity.Shipment);
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.Status).HasConversion(v => v.ToString(), v => v != null ? (ShipmentStatusEnum)Enum.Parse(typeof(ShipmentStatusEnum), v) : default)
                .HasColumnType("nvarchar(20)");
            });
            //seed product
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.Parse("2a9394e2-52b3-46d5-8a33-af4d6020e440"),
                    Name = "Product 1",
                    Description = "Description for Product 1",
                    Price = 10000,
                    Inventory = 100,
                    CategoryId = Guid.Parse("5F18BF0C-7199-462C-B023-3CCF1FD9F806"),
                    Gender = GenderEnum.Male,
                    CreatedAt = DateTime.Parse("2024-10-11")
                },
                new Product
                {
                    Id = Guid.Parse("8657ed40-1b9d-44e2-800d-40bb1a20af98"),
                    Name = "Product 2",
                    Description = "Description for Product 2",
                    Price = 20000,
                    Inventory = 50,
                    CategoryId = Guid.Parse("3D4FC185-049D-4A96-851B-1D320E7DBBA8"),
                    Gender = GenderEnum.Female,
                    CreatedAt = DateTime.Parse("2024-10-11"),
                    IsForSell = true
                },
                new Product
                {
                    Id = Guid.Parse("f3b3b3b4-1b9d-44e2-800d-40bb1a20af98"),
                    Name = "Product 3",
                    Description = "Description for Product 3",
                    Price = 30000,
                    Inventory = 150,
                    CategoryId = Guid.Parse("9A17DCF5-1426-45EE-A32E-C23EE5FE40D9"),
                    Gender = GenderEnum.Male,
                    CreatedAt = DateTime.Parse("2024-10-11"),
                    IsForSell = true
                }
            );

            //config cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(entity => entity.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => v != null ? (CartStatusEnum)Enum.Parse(typeof(CartStatusEnum), v) : default)
                .HasColumnType("nvarchar(20)");
                entity.HasOne(entity => entity.User)
                .WithMany(entity => entity.Carts)
                .HasForeignKey(entity => entity.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            //seed cart
            modelBuilder.Entity<Cart>().HasData(
                new Cart
                {
                    Id = Guid.Parse("37ab9331-f39a-4072-80ad-4adc3684fcec"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Status = CartStatusEnum.Active,
                    CreatedAt = DateTime.Parse("2024-10-11")
                },
                new Cart
                {
                    Id = Guid.Parse("da17c01a-de60-4b46-810e-f824a1936e14"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Status = CartStatusEnum.Completed,
                    CreatedAt = DateTime.Parse("2024-10-11")
                }
            );
            //config cart item
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.HasOne(entity => entity.Product)
                .WithMany(entity => entity.CartItems)
                .HasForeignKey(entity => entity.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //config order item
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.HasOne(entity => entity.Product)
                .WithMany(entity => entity.OrderItems)
                .HasForeignKey(entity => entity.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //config order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(entity => entity.Shipment)
                .WithOne(entity => entity.Order);
                entity.Property(entity => entity.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => v != null ? (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), v) : default);

                entity.Property(entity => entity.PaymentMethod)
                .HasConversion(
                    v => v.ToString(),
                    v => v != null ? (PaymentMethodEnum)Enum.Parse(typeof(PaymentMethodEnum), v) : default);
                entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Potential cascade path

                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade); // Potential cascade path

                entity.HasOne(o => o.Payment)
                    .WithOne(p => p.Order)
                    .HasForeignKey<PaymentOrder>(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Restrict); // Potential cascade path

                entity.HasOne(o => o.Shipment)
                    .WithOne(s => s.Order)
                    .HasForeignKey<Shipment>(s => s.OrderId)
                    .OnDelete(DeleteBehavior.Cascade); // Potential cascade path
            });
            //config consignment
            modelBuilder.Entity<Consignment>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.Method)
                .HasConversion(
                    v => v.ToString(),
                    v => v != null ? (ConsignmentMethodEnum)Enum.Parse(typeof(ConsignmentMethodEnum), v) : default)
                .HasColumnType("nvarchar(20)");

                entity.Property(entity => entity.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => v != null ? (ConsignmentStatusEnum)Enum.Parse(typeof(ConsignmentStatusEnum), v) : default)
                .HasColumnType("nvarchar(20)");
                entity.HasOne(c => c.User)
                .WithMany(u => u.Consignments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(c => c.Product)
                .WithOne(p => p.Consignment)
                .HasForeignKey<Product>(p => p.ConsignmentId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Payment)
                .WithOne(p => p.Consignment)
                .HasForeignKey<PaymentConsignment>(p => p.ConsignmentId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //config promotion
            modelBuilder.Entity<Promotion>()
            .HasIndex(p => p.DiscountCode)
            .IsUnique();
            //config batch
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.IsForSell).HasDefaultValue(false);
                entity.HasOne(b => b.Product)
                .WithMany(p => p.Batches)
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //config user
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.RoleId).HasDefaultValue(RoleConst.CUSTOMER_ID);
                entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Wallet)
                .WithOne(w => w.Owner)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            //config media
            modelBuilder.Entity<Media>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => v != null ? (MediaTypeEnum)Enum.Parse(typeof(MediaTypeEnum), v) : default);
            });
        }
    }
}