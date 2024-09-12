using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed the Category table
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = Guid.Parse("5F18BF0C-7199-462C-B023-3CCF1FD9F806"), Name = "Thuần chủng nhập khẩu" },
                new Category { Id = Guid.Parse("3D4FC185-049D-4A96-851B-1D320E7DBBA8"), Name = "Lai F1" },
                new Category { Id = Guid.Parse("9A17DCF5-1426-45EE-A32E-C23EE5FE40D9"), Name = "Thuần Việt" }
            );
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(entity => entity.Gender).HasConversion(v => v.ToString(), v => v != null ? (GenderEnum)Enum.Parse(typeof(GenderEnum), v) : default)
                .HasColumnType("nvarchar(20)");
            });
            // Seed the Role table
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
            // Seed the User table
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
                    CreatedAt = DateTime.Now,
                }
                // new User
                // {
                //     Id = Guid.NewGuid(),
                //     FullName = "Jane Smith",
                //     Email
                // },
                // new User
                // {
                //     Id = Guid.NewGuid(),
                //     FullName = "Mike Johnson"
                // }
            );
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(entity => entity.Status)
                .HasConversion(
                    v => v.ToString(), 
                    v => v != null ? (CartStatusEnum)Enum.Parse(typeof(CartStatusEnum), v) : default)
                .HasColumnType("nvarchar(20)");

            });
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 1",
                    Description = "Description for Product 1",
                    Price = 100,
                    Inventory = 10,
                    CategoryId = Guid.Parse("5F18BF0C-7199-462C-B023-3CCF1FD9F806"),
                    Gender = GenderEnum.Male,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 2",
                    Description = "Description for Product 2",
                    Price = 200,
                    Inventory = 10,
                    CategoryId = Guid.Parse("3D4FC185-049D-4A96-851B-1D320E7DBBA8"),
                    Gender = GenderEnum.Female,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 3",
                    Description = "Description for Product 3",
                    Price = 300,
                    Inventory = 10,
                    CategoryId = Guid.Parse("9A17DCF5-1426-45EE-A32E-C23EE5FE40D9"),
                    Gender = GenderEnum.Male,
                    CreatedAt = DateTime.Now
                }
            );
            modelBuilder.Entity<Cart>().HasData(
                new Cart
                {
                    Id = Guid.Parse("37ab9331-f39a-4072-80ad-4adc3684fcec"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Status = CartStatusEnum.Active,
                    CreatedAt = DateTime.Now
                },
                new Cart
                {
                    Id = Guid.Parse("da17c01a-de60-4b46-810e-f824a1936e14"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Status = CartStatusEnum.Completed,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}