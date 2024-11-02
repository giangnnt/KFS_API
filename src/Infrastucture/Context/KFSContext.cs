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
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //seed role
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    Name = "ADMIN"
                },
                new Role
                {
                    RoleId = 2,
                    Name = "MANAGER"
                },
                new Role
                {
                    RoleId = 3,
                    Name = "STAFF"
                },
                new Role
                {
                    RoleId = 4,
                    Name = "CUSTOMER"
                },
                new Role
                {
                    RoleId = 5,
                    Name = "GUEST"
                }
            );
            //seed permission
            modelBuilder.Entity<Permission>()
            .HasData(
                new Permission { Slug = PermissionSlug.MANAGE_ADDRESS, Name = "Manage Address" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_ADDRESS, Name = "Manage Own Address" },
                new Permission { Slug = PermissionSlug.VIEW_ADDRESS, Name = "View Address" },
                new Permission { Slug = PermissionSlug.MANAGE_BATCH, Name = "Manage Batch" },
                new Permission { Slug = PermissionSlug.VIEW_BATCH, Name = "View Batch" },
                new Permission { Slug = PermissionSlug.MANAGE_CART, Name = "Manage Cart" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_CART, Name = "Manage Own Cart" },
                new Permission { Slug = PermissionSlug.VIEW_CART, Name = "View Cart" },
                new Permission { Slug = PermissionSlug.MANAGE_CATEGORY, Name = "Manage Category" },
                new Permission { Slug = PermissionSlug.VIEW_CATEGORY, Name = "View Category" },
                new Permission { Slug = PermissionSlug.MANAGE_CONSIGNMENT, Name = "Manage Consignment" },
                new Permission { Slug = PermissionSlug.UPDATE_CONSIGNMENT, Name = "Update Consignment" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_CONSIGNMENT, Name = "Manage Own Consignment" },
                new Permission { Slug = PermissionSlug.VIEW_CONSIGNMENT, Name = "View Consignment" },
                new Permission { Slug = PermissionSlug.MANAGE_CREDENTIAL, Name = "Manage Credential" },
                new Permission { Slug = PermissionSlug.VIEW_CREDENTIAL, Name = "View Credential" },
                new Permission { Slug = PermissionSlug.MANAGE_FEEDBACK, Name = "Manage Feedback" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_FEEDBACK, Name = "Manage Own Feedback" },
                new Permission { Slug = PermissionSlug.VIEW_FEEDBACK, Name = "View Feedback" },
                new Permission { Slug = PermissionSlug.MANAGE_MEDIA, Name = "Manage Media" },
                new Permission { Slug = PermissionSlug.VIEW_MEDIA, Name = "View Media" },
                new Permission { Slug = PermissionSlug.MANAGE_ORDER, Name = "Manage Order" },
                new Permission { Slug = PermissionSlug.UPDATE_ORDER, Name = "Update Order" },
                new Permission { Slug = PermissionSlug.CREATE_ORDER_OFFLINE, Name = "Create Order Offline" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_ORDER, Name = "Manage Own Order" },
                new Permission { Slug = PermissionSlug.VIEW_ORDER, Name = "View Order" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_PAYMENT, Name = "Manage Own Payment" },
                new Permission { Slug = PermissionSlug.MANAGE_PAYMENT, Name = "Manage Payment" },
                new Permission { Slug = PermissionSlug.VIEW_PAYMENT, Name = "View Payment" },
                new Permission { Slug = PermissionSlug.MANAGE_PERMISSION, Name = "Manage Permission" },
                new Permission { Slug = PermissionSlug.VIEW_PERMISSION, Name = "View Permission" },
                new Permission { Slug = PermissionSlug.MANAGE_PRODUCT, Name = "Manage Product" },
                new Permission { Slug = PermissionSlug.VIEW_PRODUCT, Name = "View Product" },
                new Permission { Slug = PermissionSlug.MANAGE_PROMOTION, Name = "Manage Promotion" },
                new Permission { Slug = PermissionSlug.VIEW_PROMOTION, Name = "View Promotion" },
                new Permission { Slug = PermissionSlug.MANAGE_ROLE, Name = "Manage Role" },
                new Permission { Slug = PermissionSlug.VIEW_ROLE, Name = "View Role" },
                new Permission { Slug = PermissionSlug.MANAGE_SHIPMENT, Name = "Manage Shipment" },
                new Permission { Slug = PermissionSlug.VIEW_SHIPPING, Name = "View Shipping" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_SHIPMENT, Name = "Manage Own Shipment" },
                new Permission { Slug = PermissionSlug.MANAGE_USER, Name = "Manage User" },
                new Permission { Slug = PermissionSlug.VIEW_USER, Name = "View User" },
                new Permission { Slug = PermissionSlug.MANAGE_OWN_USER, Name = "Manage Own User" },
                new Permission { Slug = PermissionSlug.MANAGE_WALLET, Name = "Manage Wallet" },
                new Permission { Slug = PermissionSlug.CREATE_PAYMENT_OFFLINE, Name = "Create Payment Offline" }
            );
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j
                .HasData(
                    // Admin
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_ADDRESS },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_ADDRESS },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_ADDRESS },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_BATCH },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_BATCH },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_CART },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_CART },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_CART },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_CATEGORY },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_CATEGORY },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_CONSIGNMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.UPDATE_CONSIGNMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_CONSIGNMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_CONSIGNMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_CREDENTIAL },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_CREDENTIAL },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_FEEDBACK },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_FEEDBACK },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_FEEDBACK },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_MEDIA },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_MEDIA },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_ORDER },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.UPDATE_ORDER },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.CREATE_ORDER_OFFLINE },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_ORDER },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_ORDER },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_PAYMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_PAYMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.CREATE_PAYMENT_OFFLINE },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_PAYMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_PERMISSION },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_PERMISSION },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_PRODUCT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_PRODUCT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_PROMOTION },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_PROMOTION },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_ROLE },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_ROLE },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_SHIPMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_SHIPPING },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_SHIPMENT },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_USER },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.VIEW_USER },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_OWN_USER },
                    new { RolesRoleId = 1, PermissionsSlug = PermissionSlug.MANAGE_WALLET },
                    // Manager
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_ADDRESS },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_BATCH },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_CATEGORY },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_CONSIGNMENT },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_CREDENTIAL },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_FEEDBACK },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_MEDIA },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_ORDER },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_PAYMENT },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_PRODUCT },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_PROMOTION },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_SHIPMENT },
                    new { RolesRoleId = 2, PermissionsSlug = PermissionSlug.MANAGE_USER },
                    // Staff
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.VIEW_ADDRESS },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.MANAGE_BATCH },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.MANAGE_CATEGORY },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.UPDATE_CONSIGNMENT },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.MANAGE_CREDENTIAL },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.VIEW_FEEDBACK },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.MANAGE_MEDIA },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.UPDATE_ORDER },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.CREATE_ORDER_OFFLINE },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.VIEW_PAYMENT },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.MANAGE_PRODUCT },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.MANAGE_PROMOTION },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.VIEW_ROLE },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.MANAGE_SHIPMENT },
                    new { RolesRoleId = 3, PermissionsSlug = PermissionSlug.VIEW_USER },
                    // Customer
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_ADDRESS },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.VIEW_BATCH },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_CART },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.VIEW_CATEGORY },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_CONSIGNMENT },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.VIEW_CONSIGNMENT },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_FEEDBACK },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.VIEW_MEDIA },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_ORDER },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_PAYMENT },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.VIEW_PRODUCT },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.VIEW_PROMOTION },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_SHIPMENT},
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_OWN_USER },
                    new { RolesRoleId = 4, PermissionsSlug = PermissionSlug.MANAGE_WALLET }
                    // Guest
                ));
            // Seed user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    FullName = "admin",
                    Email = "admin@gmail.com",
                    IsActive = true,
                    RoleId = 1,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Phone = "0123456789",
                    CreatedAt = DateTime.Parse("2024-10-11"),
                },
                new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    FullName = "manager",
                    Email = "manager@gmail.com",
                    IsActive = true,
                    RoleId = 2,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Phone = "0123456789",
                    CreatedAt = DateTime.Parse("2024-10-11"),
                },
                new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    FullName = "staff",
                    Email = "staff@gmail.com",
                    IsActive = true,
                    RoleId = 3,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Phone = "0123456789",
                    CreatedAt = DateTime.Parse("2024-10-11"),
                },
                new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    FullName = "customer",
                    Email = "customer@gmail.com",
                    IsActive = true,
                    RoleId = 4,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Phone = "0123456789",
                    CreatedAt = DateTime.Parse("2024-10-11"),
                },
                new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    FullName = "guest",
                    Email = "guest@gmail.com",
                    IsActive = true,
                    RoleId = 5,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Phone = "0123456789",
                    CreatedAt = DateTime.Parse("2024-10-11"),
                }
            );
            // Seed Address
            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Street = "77c Huynh Thi Tuoi",
                    WardCode = "440504",
                    DistrictId = 1540,
                    ProvinceId = 205,
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                },
                new Address
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Street = "shop address",
                    WardCode = "440504",
                    DistrictId = 1540,
                    ProvinceId = 205,
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001")
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
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
                entity.Property(entity => entity.Status).HasConversion(v => v.ToString(), v => v != null ? (ProductStatusEnum)Enum.Parse(typeof(ProductStatusEnum), v) : default);
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
                    Weight = 10000,
                    Inventory = 10,
                    Price = 10000,
                    IsForSell = true
                },
                new Batch
                {
                    Id = Guid.Parse("eb5707d1-1b02-4ad5-8d6b-27a78d00f322"),
                    Name = "Batch 2",
                    ProductId = Guid.Parse("8657ed40-1b9d-44e2-800d-40bb1a20af98"),
                    Quantity = 10,
                    Inventory = 10,
                    Weight = 20000,
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
                    Weight = 20000,
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
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
                entity.Property(entity => entity.PaymentMethod).HasConversion(v => v.ToString(), v => v != null ? (PaymentMethodEnum)Enum.Parse(typeof(PaymentMethodEnum), v) : default)
                .HasColumnType("nvarchar(20)");
            });
            //config shipment
            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
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
                    Weight = 1000,
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
                    Weight = 2000,
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
                    Weight = 3000,
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
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
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
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
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

                entity.HasOne(o => o.Address)
                    .WithMany(a => a.Orders)
                    .HasForeignKey(o => o.AddressId)
                    .OnDelete(DeleteBehavior.Restrict); // Potential cascade path
            });
            //config consignment
            modelBuilder.Entity<Consignment>(entity =>
            {
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
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
            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
            });
            //config batch
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.IsForSell).HasDefaultValue(false);
                entity.Property(entity => entity.Status).HasConversion(v => v.ToString(), v => v != null ? (ProductStatusEnum)Enum.Parse(typeof(ProductStatusEnum), v) : default);
                entity.HasOne(b => b.Product)
                .WithMany(p => p.Batches)
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //config user
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Wallet)
                .WithOne(w => w.Owner)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //config media
            modelBuilder.Entity<Media>(entity =>
            {
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.Property(entity => entity.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => v != null ? (MediaTypeEnum)Enum.Parse(typeof(MediaTypeEnum), v) : default);
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
            //config feedback
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(entity => entity.CreatedAt).HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();
                entity.HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(f => f.Product)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}