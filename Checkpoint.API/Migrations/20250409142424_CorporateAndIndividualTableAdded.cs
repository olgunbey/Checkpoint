using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Checkpoint.API.Migrations
{
    /// <inheritdoc />
    public partial class CorporateAndIndividualTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorporateId",
                table: "Project",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndividualId",
                table: "Project",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Corporate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mail = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Individual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individual", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_CorporateId",
                table: "Project",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_IndividualId",
                table: "Project",
                column: "IndividualId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Corporate_CorporateId",
                table: "Project",
                column: "CorporateId",
                principalTable: "Corporate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Individual_IndividualId",
                table: "Project",
                column: "IndividualId",
                principalTable: "Individual",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Corporate_CorporateId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Individual_IndividualId",
                table: "Project");

            migrationBuilder.DropTable(
                name: "Corporate");

            migrationBuilder.DropTable(
                name: "Individual");

            migrationBuilder.DropIndex(
                name: "IX_Project_CorporateId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_IndividualId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CorporateId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "IndividualId",
                table: "Project");
        }
    }
}
