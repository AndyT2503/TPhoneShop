using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.CreateTable(
                name: "coupons",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaximumDiscountAmount_Amount = table.Column<long>(type: "bigint", nullable: true),
                    MaximumDiscountAmount_Currency = table.Column<string>(type: "text", nullable: true),
                    MinimumOrderAmount_Amount = table.Column<long>(type: "bigint", nullable: true),
                    MinimumOrderAmount_Currency = table.Column<string>(type: "text", nullable: true),
                    UsageLimit = table.Column<int>(type: "integer", nullable: true),
                    PerUserUsageLimit = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    StartsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingAddress_RecipientName = table.Column<string>(type: "text", nullable: true),
                    ShippingAddress_PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    ShippingAddress_Email = table.Column<string>(type: "text", nullable: true),
                    ShippingAddress_Province = table.Column<string>(type: "text", nullable: true),
                    ShippingAddress_Ward = table.Column<string>(type: "text", nullable: true),
                    ShippingAddress_Address = table.Column<string>(type: "text", nullable: true),
                    CustomerNote = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CancelReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TotalDiscount_Amount = table.Column<long>(type: "bigint", nullable: true),
                    TotalDiscount_Currency = table.Column<string>(type: "text", nullable: true),
                    ShippingFee_Amount = table.Column<long>(type: "bigint", nullable: true),
                    ShippingFee_Currency = table.Column<string>(type: "text", nullable: true),
                    Tax_Amount = table.Column<long>(type: "bigint", nullable: false),
                    Tax_Currency = table.Column<string>(type: "text", nullable: false),
                    SubTotal_Amount = table.Column<long>(type: "bigint", nullable: false),
                    SubTotal_Currency = table.Column<string>(type: "text", nullable: false),
                    TotalAmount_Amount = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount_Currency = table.Column<string>(type: "text", nullable: false),
                    ShippedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PaidAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "order_discounts",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AppliedAmount_Amount = table.Column<long>(type: "bigint", nullable: false),
                    AppliedAmount_Currency = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_discounts_coupons_CouponId",
                        column: x => x.CouponId,
                        principalSchema: "orders",
                        principalTable: "coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_discounts_orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductVariantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UnitPrice_Amount = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice_Currency = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    SubTotal_Amount = table.Column<long>(type: "bigint", nullable: false),
                    SubTotal_Currency = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_items_orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_items_product_variants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalSchema: "catalogs",
                        principalTable: "product_variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_logs",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShippingMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PerformedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    PerfomedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_logs_orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_coupons_Code",
                schema: "orders",
                table: "coupons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coupons_IsActive",
                schema: "orders",
                table: "coupons",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_IsActive_StartsAt_ExpiresAt",
                schema: "orders",
                table: "coupons",
                columns: new[] { "IsActive", "StartsAt", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_order_discounts_Code",
                schema: "orders",
                table: "order_discounts",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_order_discounts_CouponId",
                schema: "orders",
                table: "order_discounts",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_order_discounts_OrderId",
                schema: "orders",
                table: "order_discounts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_OrderId",
                schema: "orders",
                table: "order_items",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_ProductVariantId",
                schema: "orders",
                table: "order_items",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_order_logs_Action",
                schema: "orders",
                table: "order_logs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_order_logs_OrderId",
                schema: "orders",
                table: "order_logs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_logs_OrderId_PerfomedAt",
                schema: "orders",
                table: "order_logs",
                columns: new[] { "OrderId", "PerfomedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_orders_CreatedAt",
                schema: "orders",
                table: "orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_orders_CustomerId",
                schema: "orders",
                table: "orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_OrderNumber",
                schema: "orders",
                table: "orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_PaymentStatus",
                schema: "orders",
                table: "orders",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Status",
                schema: "orders",
                table: "orders",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_discounts",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "order_items",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "order_logs",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "coupons",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders");
        }
    }
}
