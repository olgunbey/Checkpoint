using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Checkpoint.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class RoleAndPermissionConnectedToTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyPermission");

            migrationBuilder.DropTable(
                name: "CompanyRole");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Team",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreateUserId",
                table: "Role",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Role",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreateUserId",
                table: "Permission",
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
                name: "IX_Team_CompanyId",
                table: "Team",
                column: "CompanyId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Company_CompanyId",
                table: "Team",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Team_TeamId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Team_TeamId",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Company_CompanyId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_CompanyId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Role_TeamId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Permission_TeamId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Permission");

            migrationBuilder.CreateTable(
                name: "CompanyPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPermission_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyRole_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermission_CompanyId",
                table: "CompanyPermission",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPermission_PermissionId",
                table: "CompanyPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRole_CompanyId",
                table: "CompanyRole",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRole_RoleId",
                table: "CompanyRole",
                column: "RoleId");
        }
    }
}
