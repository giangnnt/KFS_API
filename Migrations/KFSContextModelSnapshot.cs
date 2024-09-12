﻿// <auto-generated />
using System;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KFS.Migrations
{
    [DbContext(typeof(KFSContext))]
    partial class KFSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KFS.src.Domain.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("TotalItem")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("37ab9331-f39a-4072-80ad-4adc3684fcec"),
                            CreatedAt = new DateTime(2024, 9, 12, 21, 21, 50, 389, DateTimeKind.Local).AddTicks(6465),
                            Currency = "VND",
                            Status = "Active",
                            TotalItem = 0,
                            TotalPrice = 0m,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = new Guid("00000000-0000-0000-0000-000000000001")
                        },
                        new
                        {
                            Id = new Guid("da17c01a-de60-4b46-810e-f824a1936e14"),
                            CreatedAt = new DateTime(2024, 9, 12, 21, 21, 50, 389, DateTimeKind.Local).AddTicks(6468),
                            Currency = "VND",
                            Status = "Completed",
                            TotalItem = 0,
                            TotalPrice = 0m,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserId = new Guid("00000000-0000-0000-0000-000000000001")
                        });
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5f18bf0c-7199-462c-b023-3ccf1fd9f806"),
                            Name = "Thuần chủng nhập khẩu"
                        },
                        new
                        {
                            Id = new Guid("3d4fc185-049d-4a96-851b-1d320e7dbba8"),
                            Name = "Lai F1"
                        },
                        new
                        {
                            Id = new Guid("9a17dcf5-1426-45ee-a32e-c23ee5fe40d9"),
                            Name = "Thuần Việt"
                        });
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Discount")
                        .HasColumnType("real");

                    b.Property<DateTime>("EstimatedDeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShippingFee")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalItem")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Permission", b =>
                {
                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Slug");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FeedingVolumn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("FilterRate")
                        .HasColumnType("real");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Inventory")
                        .HasColumnType("int");

                    b.Property<float?>("Length")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Species")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("68ac40db-f025-409d-88d6-76cd26892c9b"),
                            CategoryId = new Guid("5f18bf0c-7199-462c-b023-3ccf1fd9f806"),
                            CreatedAt = new DateTime(2024, 9, 12, 21, 21, 50, 389, DateTimeKind.Local).AddTicks(6352),
                            Description = "Description for Product 1",
                            Gender = "Male",
                            Inventory = 10,
                            Name = "Product 1",
                            Price = 100m,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("79b6a852-35ff-4a75-b648-2f885fe50e2c"),
                            CategoryId = new Guid("3d4fc185-049d-4a96-851b-1d320e7dbba8"),
                            CreatedAt = new DateTime(2024, 9, 12, 21, 21, 50, 389, DateTimeKind.Local).AddTicks(6417),
                            Description = "Description for Product 2",
                            Gender = "Female",
                            Inventory = 10,
                            Name = "Product 2",
                            Price = 200m,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("337b5cf7-2f08-48e7-a05d-530884405a42"),
                            CategoryId = new Guid("9a17dcf5-1426-45ee-a32e-c23ee5fe40d9"),
                            CreatedAt = new DateTime(2024, 9, 12, 21, 21, 50, 389, DateTimeKind.Local).AddTicks(6421),
                            Description = "Description for Product 3",
                            Gender = "Male",
                            Inventory = 10,
                            Name = "Product 3",
                            Price = 300m,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            Name = "ADMIN"
                        },
                        new
                        {
                            RoleId = 2,
                            Name = "MANAGER"
                        },
                        new
                        {
                            RoleId = 3,
                            Name = "STAFF"
                        },
                        new
                        {
                            RoleId = 4,
                            Name = "CUSTOMER"
                        },
                        new
                        {
                            RoleId = 5,
                            Name = "GUEST"
                        });
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Address = "HCM",
                            CreatedAt = new DateTime(2024, 9, 12, 21, 21, 50, 389, DateTimeKind.Local).AddTicks(3747),
                            Email = "giangnnt260703@gmail.com",
                            FullName = "Truong Giang",
                            Password = "$2a$11$Nbcz8f3m7VROikIdEYeXs.mlcBYLNFnQXg4VubSgrDGLSTkgnAfUe",
                            Phone = "0123456789",
                            RoleId = 1,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<string>("PermissionsSlug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RolesRoleId")
                        .HasColumnType("int");

                    b.HasKey("PermissionsSlug", "RolesRoleId");

                    b.HasIndex("RolesRoleId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Cart", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.CartItem", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KFS.src.Domain.Entities.Product", "Product")
                        .WithMany("CartItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Order", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KFS.src.Domain.Entities.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Payment", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("KFS.src.Domain.Entities.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Product", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.User", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("KFS.src.Domain.Entities.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsSlug")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KFS.src.Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Product", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("KFS.src.Domain.Entities.User", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
