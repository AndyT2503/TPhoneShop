using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogoUrlToLogoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                schema: "catalogs",
                table: "brands");

            migrationBuilder.AddColumn<Guid>(
                name: "LogoId",
                schema: "catalogs",
                table: "brands",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoId",
                schema: "catalogs",
                table: "brands");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                schema: "catalogs",
                table: "brands",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
