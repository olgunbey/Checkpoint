using System.Collections.Generic;
using Checkpoint.API.RequestPayloads;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Checkpoint.API.Migrations
{
    /// <inheritdoc />
    public partial class BaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: true),
                    IndividualId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestedEndpointId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedEndpointId", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseUrl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BasePath = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseUrl_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Controller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ControllerPath = table.Column<string>(type: "text", nullable: false),
                    BaseUrlId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Controller_BaseUrl_BaseUrlId",
                        column: x => x.BaseUrlId,
                        principalTable: "BaseUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionPath = table.Column<string>(type: "text", nullable: false),
                    ControllerId = table.Column<int>(type: "integer", nullable: false),
                    RequestType = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<List<Body>>(type: "jsonb", nullable: true),
                    Header = table.Column<List<Header>>(type: "jsonb", nullable: true),
                    Query = table.Column<List<Query>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Controller_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Controller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "IndividualId", "ProjectName", "TeamId" },
                values: new object[,]
                {
                    { 1, null, "Job Projesi", 1 },
                    { 2, null, "Otoyol Projesi", 1 }
                });

            migrationBuilder.InsertData(
                table: "BaseUrl",
                columns: new[] { "Id", "BasePath", "ProjectId" },
                values: new object[,]
                {
                    { 1, "https://localhost:5000/api", 1 },
                    { 2, "https://localhost:5001/api", 2 }
                });

            migrationBuilder.InsertData(
                table: "Controller",
                columns: new[] { "Id", "BaseUrlId", "ControllerPath" },
                values: new object[,]
                {
                    { 1, 1, "User" },
                    { 2, 1, "Teacher" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ControllerId",
                table: "Action",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUrl_ProjectId",
                table: "BaseUrl",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Controller_BaseUrlId",
                table: "Controller",
                column: "BaseUrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "RequestedEndpointId");

            migrationBuilder.DropTable(
                name: "Controller");

            migrationBuilder.DropTable(
                name: "BaseUrl");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
