using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checkpoint.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class RoleAndPermissionsNotConnecToTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Team_TeamId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Team_TeamId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_TeamId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Permission_TeamId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Permission");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Role",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Permission",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Role_TeamId",
                table: "Role",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_TeamId",
                table: "Permission",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Team_TeamId",
                table: "Permission",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Team_TeamId",
                table: "Role",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
