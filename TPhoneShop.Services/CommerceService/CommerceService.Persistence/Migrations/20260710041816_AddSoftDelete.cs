using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "catalogs",
                table: "products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "catalogs",
                table: "products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "catalogs",
                table: "product_variants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "catalogs",
                table: "product_variants",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "catalogs",
                table: "categories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "catalogs",
                table: "categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                schema: "catalogs",
                table: "brands",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "catalogs",
                table: "brands",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalogs",
                table: "products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalogs",
                table: "products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalogs",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalogs",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalogs",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalogs",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "catalogs",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "catalogs",
                table: "brands");
        }
    }
}
