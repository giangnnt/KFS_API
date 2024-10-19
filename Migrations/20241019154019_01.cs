using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KFS.Migrations
{
    /// <inheritdoc />
    public partial class _01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Slug);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiscountPercentage = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPromotion",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPromotion", x => new { x.CategoriesId, x.PromotionsId });
                    table.ForeignKey(
                        name: "FK_CategoryPromotion_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPromotion_Promotions_PromotionsId",
                        column: x => x.PromotionsId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsSlug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RolesRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsSlug, x.RolesRoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_PermissionsSlug",
                        column: x => x.PermissionsSlug,
                        principalTable: "Permissions",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false, defaultValue: 4),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalItem = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CommissionPercentage = table.Column<int>(type: "int", nullable: true),
                    DealingAmount = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    IsForSell = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConsignmentFee = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBatch = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalItem = table.Column<int>(type: "int", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingFee = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDeliveryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Length = table.Column<float>(type: "real", nullable: true),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedingVolumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilterRate = table.Column<float>(type: "real", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Inventory = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsForSell = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Consignments_ConsignmentId",
                        column: x => x.ConsignmentId,
                        principalTable: "Consignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ConsignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Consignments_ConsignmentId",
                        column: x => x.ConsignmentId,
                        principalTable: "Consignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Inventory = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsForSell = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batches_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsBatch = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredentialFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credentials_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaProduct",
                columns: table => new
                {
                    MediasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaProduct", x => new { x.MediasId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_MediaProduct_Medias_MediasId",
                        column: x => x.MediasId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsBatch = table.Column<bool>(type: "bit", nullable: false),
                    IsConsignment = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPromotion",
                columns: table => new
                {
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPromotion", x => new { x.ProductsId, x.PromotionsId });
                    table.ForeignKey(
                        name: "FK_ProductPromotion_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPromotion_Promotions_PromotionsId",
                        column: x => x.PromotionsId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchPromotion",
                columns: table => new
                {
                    BatchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchPromotion", x => new { x.BatchesId, x.PromotionsId });
                    table.ForeignKey(
                        name: "FK_BatchPromotion_Batches_BatchesId",
                        column: x => x.BatchesId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchPromotion_Promotions_PromotionsId",
                        column: x => x.PromotionsId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Image", "Name" },
                values: new object[,]
                {
                    { new Guid("3d4fc185-049d-4a96-851b-1d320e7dbba8"), null, null, "Lai F1" },
                    { new Guid("5f18bf0c-7199-462c-b023-3ccf1fd9f806"), null, null, "Thuần chủng nhập khẩu" },
                    { new Guid("9a17dcf5-1426-45ee-a32e-c23ee5fe40d9"), null, null, "Thuần Việt" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Slug", "Description", "Name" },
                values: new object[,]
                {
                    { "FEEDBACK.ALL", null, "Manage Feedback" },
                    { "FEEDBACK.OWN", null, "Manage Own Feedback" },
                    { "MANAGE_CATEGORY.ALL", null, "Manage Category" },
                    { "MANAGE_ORDER.ALL", null, "Manage Order" },
                    { "PERMISSION.ALL", null, "Manage Permission" },
                    { "PRODUCT.ALL", null, "Manage Product" },
                    { "ROLE.ALL", null, "Manage Role" },
                    { "USER.ALL", null, "Manage User" },
                    { "USER.OWN", null, "Manage Own User" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "ADMIN" },
                    { 2, null, "MANAGER" },
                    { 3, null, "STAFF" },
                    { 4, null, "CUSTOMER" },
                    { 5, null, "GUEST" }
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionsSlug", "RolesRoleId" },
                values: new object[,]
                {
                    { "FEEDBACK.ALL", 1 },
                    { "FEEDBACK.OWN", 1 },
                    { "MANAGE_CATEGORY.ALL", 1 },
                    { "MANAGE_ORDER.ALL", 1 },
                    { "PERMISSION.ALL", 1 },
                    { "PRODUCT.ALL", 1 },
                    { "ROLE.ALL", 1 },
                    { "USER.ALL", 1 },
                    { "USER.OWN", 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Age", "CategoryId", "Color", "ConsignmentId", "CreatedAt", "Description", "FeedingVolumn", "FilterRate", "Gender", "Inventory", "IsForSell", "Length", "Name", "Origin", "Price", "Species", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2a9394e2-52b3-46d5-8a33-af4d6020e440"), null, new Guid("5f18bf0c-7199-462c-b023-3ccf1fd9f806"), null, null, new DateTime(2024, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description for Product 1", null, null, "Male", 100, false, null, "Product 1", null, 10000m, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8657ed40-1b9d-44e2-800d-40bb1a20af98"), null, new Guid("3d4fc185-049d-4a96-851b-1d320e7dbba8"), null, null, new DateTime(2024, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description for Product 2", null, null, "Female", 50, true, null, "Product 2", null, 20000m, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f3b3b3b4-1b9d-44e2-800d-40bb1a20af98"), null, new Guid("9a17dcf5-1426-45ee-a32e-c23ee5fe40d9"), null, null, new DateTime(2024, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Description for Product 3", null, null, "Male", 150, true, null, "Product 3", null, 30000m, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Avatar", "CreatedAt", "Email", "FullName", "Password", "Phone", "RoleId", "UpdatedAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "HCM", null, new DateTime(2024, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "giangnnt260703@gmail.com", "Truong Giang", "$2a$11$zG546U333pIZBzJ8m2GXtuedAk8HlCRabnATYC1gZcepLlg4VXWkK", "0123456789", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Batches",
                columns: new[] { "Id", "Description", "Inventory", "Name", "Price", "ProductId", "Quantity", "Status" },
                values: new object[] { new Guid("64141e04-9c9d-4bd1-94d2-84ee359f1e5b"), null, 10, "Batch 1", 10000m, new Guid("2a9394e2-52b3-46d5-8a33-af4d6020e440"), 10, 0 });

            migrationBuilder.InsertData(
                table: "Batches",
                columns: new[] { "Id", "Description", "Inventory", "IsForSell", "Name", "Price", "ProductId", "Quantity", "Status" },
                values: new object[,]
                {
                    { new Guid("b6bfe977-ee7e-4e84-a65b-445c36d08d65"), null, 10, true, "Batch 3", 30000m, new Guid("f3b3b3b4-1b9d-44e2-800d-40bb1a20af98"), 10, 0 },
                    { new Guid("eb5707d1-1b02-4ad5-8d6b-27a78d00f322"), null, 10, true, "Batch 2", 20000m, new Guid("8657ed40-1b9d-44e2-800d-40bb1a20af98"), 10, 0 }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "CreatedAt", "Currency", "Status", "TotalItem", "TotalPrice", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("37ab9331-f39a-4072-80ad-4adc3684fcec"), new DateTime(2024, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "VND", "Active", 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("da17c01a-de60-4b46-810e-f824a1936e14"), new DateTime(2024, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "VND", "Completed", 0, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Point", "UserId" },
                values: new object[] { new Guid("943e7be4-5643-4b56-bc30-78415c17a0ae"), 20000, new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_ProductId",
                table: "Batches",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchPromotion_PromotionsId",
                table: "BatchPromotion",
                column: "PromotionsId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPromotion_PromotionsId",
                table: "CategoryPromotion",
                column: "PromotionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Consignments_UserId",
                table: "Consignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_ProductId",
                table: "Credentials",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ProductId",
                table: "Feedbacks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaProduct_ProductsId",
                table: "MediaProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ConsignmentId",
                table: "Payments",
                column: "ConsignmentId",
                unique: true,
                filter: "[ConsignmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RolesRoleId",
                table: "PermissionRole",
                column: "RolesRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPromotion_PromotionsId",
                table: "ProductPromotion",
                column: "PromotionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ConsignmentId",
                table: "Products",
                column: "ConsignmentId",
                unique: true,
                filter: "[ConsignmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_DiscountCode",
                table: "Promotions",
                column: "DiscountCode",
                unique: true,
                filter: "[DiscountCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchPromotion");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CategoryPromotion");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "MediaProduct");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "ProductPromotion");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Consignments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
