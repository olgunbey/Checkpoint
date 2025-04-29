using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checkpoint.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class migRolTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Role",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Role_TeamId",
                table: "Role",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Team_TeamId",
                table: "Role",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Team_TeamId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_TeamId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Role");
        }
    }
}
