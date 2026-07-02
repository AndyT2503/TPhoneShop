using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialFileDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    ReferrenceId = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medias", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_medias_Key",
                table: "medias",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_medias_ReferrenceId",
                table: "medias",
                column: "ReferrenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "medias");
        }
    }
}
