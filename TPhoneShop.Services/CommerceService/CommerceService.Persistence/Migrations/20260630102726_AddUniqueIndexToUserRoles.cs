using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommerceService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_user_roles_RoleId_UserId",
                schema: "auth",
                table: "user_roles",
                columns: new[] { "RoleId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_roles_RoleId_UserId",
                schema: "auth",
                table: "user_roles");
        }
    }
}
