using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeThumbnailUrlToThumbnailId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                schema: "catalogs",
                table: "product_variants");

            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailId",
                schema: "catalogs",
                table: "product_variants",
                type: "uuid",
                maxLength: 1000,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                schema: "catalogs",
                table: "product_variants");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                schema: "catalogs",
                table: "product_variants",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
