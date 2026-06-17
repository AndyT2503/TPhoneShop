using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredActivatedAtToSigningKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ActivatedAt",
                table: "signing_keys",
                type: "timestamp with time zone",
                nullable: false,
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ActivatedAt",
                table: "signing_keys",
                type: "timestamp with time zone",
                nullable: true,
                oldType: "timestamp with time zone");
        }
    }
}
