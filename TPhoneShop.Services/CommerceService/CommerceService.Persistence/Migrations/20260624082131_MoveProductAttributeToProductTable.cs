using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveProductAttributeToProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attributes",
                schema: "products",
                table: "product_variants");

            migrationBuilder.AddColumn<string>(
                name: "Attributes",
                schema: "products",
                table: "products",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attributes",
                schema: "products",
                table: "products");

            migrationBuilder.AddColumn<string>(
                name: "Attributes",
                schema: "products",
                table: "product_variants",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
