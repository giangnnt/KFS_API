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
                new Category { Id = Guid.NewGuid(), Name = "Thuần chủng nhập khẩu" },
                new Category { Id = Guid.NewGuid(), Name = "Lai F1" },
                new Category { Id = Guid.NewGuid(), Name = "Thuần Việt" }
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
        }
    }
}