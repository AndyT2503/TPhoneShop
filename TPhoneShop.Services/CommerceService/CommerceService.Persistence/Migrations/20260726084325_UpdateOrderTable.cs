using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                schema: "catalogs",
                table: "product_variants",
                newName: "Price_Currency");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "catalogs",
                table: "product_variants",
                newName: "Price_Amount");

            migrationBuilder.RenameColumn(
                name: "CompareAtPrice",
                schema: "catalogs",
                table: "product_variants",
                newName: "CompareAtPrice_Amount");

            migrationBuilder.AddColumn<string>(
                name: "CompareAtPrice_Currency",
                schema: "catalogs",
                table: "product_variants",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompareAtPrice_Currency",
                schema: "catalogs",
                table: "product_variants");

            migrationBuilder.RenameColumn(
                name: "Price_Currency",
                schema: "catalogs",
                table: "product_variants",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                schema: "catalogs",
                table: "product_variants",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "CompareAtPrice_Amount",
                schema: "catalogs",
                table: "product_variants",
                newName: "CompareAtPrice");
        }
    }
}
