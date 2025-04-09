using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Checkpoint.API.Migrations
{
    /// <inheritdoc />
    public partial class RoleTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndividualId = table.Column<int>(type: "integer", nullable: true),
                    CorporateId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Corporate_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRole_Individual_IndividualId",
                        column: x => x.IndividualId,
                        principalTable: "Individual",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CorporateId",
                table: "UserRole",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_IndividualId",
                table: "UserRole",
                column: "IndividualId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
