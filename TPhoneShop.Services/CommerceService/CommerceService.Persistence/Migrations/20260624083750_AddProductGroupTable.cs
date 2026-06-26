using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductGroupId",
                schema: "products",
                table: "products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "product_groups",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_groups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_ProductGroupId",
                schema: "products",
                table: "products",
                column: "ProductGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_product_groups_Slug",
                schema: "products",
                table: "product_groups",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_products_product_groups_ProductGroupId",
                schema: "products",
                table: "products",
                column: "ProductGroupId",
                principalSchema: "products",
                principalTable: "product_groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_product_groups_ProductGroupId",
                schema: "products",
                table: "products");

            migrationBuilder.DropTable(
                name: "product_groups",
                schema: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_ProductGroupId",
                schema: "products",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ProductGroupId",
                schema: "products",
                table: "products");
        }
    }
}
