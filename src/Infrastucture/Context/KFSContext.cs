using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                entity.Property(entity => entity.Gender).HasConversion(v => v.ToString(), v => (GenderEnum)Enum.Parse(typeof(GenderEnum), v));
            });
        }
    }
}