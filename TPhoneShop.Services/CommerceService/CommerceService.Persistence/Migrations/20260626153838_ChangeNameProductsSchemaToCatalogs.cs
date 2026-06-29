using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameProductsSchemaToCatalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalogs");

            migrationBuilder.RenameTable(
                name: "products",
                schema: "products",
                newName: "products",
                newSchema: "catalogs");

            migrationBuilder.RenameTable(
                name: "product_variants",
                schema: "products",
                newName: "product_variants",
                newSchema: "catalogs");

            migrationBuilder.RenameTable(
                name: "product_groups",
                schema: "products",
                newName: "product_groups",
                newSchema: "catalogs");

            migrationBuilder.RenameTable(
                name: "categories",
                schema: "products",
                newName: "categories",
                newSchema: "catalogs");

            migrationBuilder.RenameTable(
                name: "brands",
                schema: "products",
                newName: "brands",
                newSchema: "catalogs");

            migrationBuilder.DropSchema("products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.RenameTable(
                name: "products",
                schema: "catalogs",
                newName: "products",
                newSchema: "products");

            migrationBuilder.RenameTable(
                name: "product_variants",
                schema: "catalogs",
                newName: "product_variants",
                newSchema: "products");

            migrationBuilder.RenameTable(
                name: "product_groups",
                schema: "catalogs",
                newName: "product_groups",
                newSchema: "products");

            migrationBuilder.RenameTable(
                name: "categories",
                schema: "catalogs",
                newName: "categories",
                newSchema: "products");

            migrationBuilder.RenameTable(
                name: "brands",
                schema: "catalogs",
                newName: "brands",
                newSchema: "products");
        }
    }
}
