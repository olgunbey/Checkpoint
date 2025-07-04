﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Checkpoint.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class basemig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    GrantType = table.Column<string>(type: "text", nullable: false),
                    Issuer = table.Column<string>(type: "text", nullable: false),
                    Audience = table.Column<string>(type: "text", nullable: false),
                    ClientSecret = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Individual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mail = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individual", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterOutbox",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterOutbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Corporate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Mail = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    InvitationId = table.Column<int>(type: "integer", nullable: true),
                    VerificationCode = table.Column<string>(type: "text", nullable: false),
                    Verification = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corporate_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CorporateId = table.Column<int>(type: "integer", nullable: true),
                    IndividualId = table.Column<int>(type: "integer", nullable: true),
                    TeamId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeam_Corporate_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTeam_Individual_IndividualId",
                        column: x => x.IndividualId,
                        principalTable: "Individual",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTeam_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTeamPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserTeamId = table.Column<int>(type: "integer", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeamPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeamPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeamPermission_UserTeam_UserTeamId",
                        column: x => x.UserTeamId,
                        principalTable: "UserTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTeamRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserTeamId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeamRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeamRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeamRole_UserTeam_UserTeamId",
                        column: x => x.UserTeamId,
                        principalTable: "UserTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Audience", "ClientId", "ClientSecret", "GrantType", "Issuer" },
                values: new object[] { 1, "https://localhost:5000", "checkpoint-client", "checkpointsecret", "resourceowner", "https://localhost:7253" });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "Key", "Name" },
                values: new object[] { 1, "hotmail", "Koç Sistem" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Ekleme" },
                    { 3, "Güncelleme" },
                    { 4, "Onay" },
                    { 5, "Rol Atama" },
                    { 6, "Rol Güncelleme" },
                    { 7, "Rol Silme" },
                    { 8, "Rol Ekleme" },
                    { 9, "Okuma" }
                });

            migrationBuilder.InsertData(
                table: "Corporate",
                columns: new[] { "Id", "CompanyId", "InvitationId", "Mail", "Password", "Verification", "VerificationCode" },
                values: new object[] { 1, 1, null, "olgunsahin0161@hotmail.com", "acf3d886b0df7919742e5191fd5a0a745e887e6e1e3e75985394dd217c83979589543bde8e3037e69bff84f884fc55d1cd846cb3a7dbf04b25ca104ed93d7c5b", false, "A123-B213" });

            migrationBuilder.InsertData(
                table: "Team",
                columns: new[] { "Id", "CompanyId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Sigorta" },
                    { 2, 1, "Araç kiralama" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name", "TeamId" },
                values: new object[] { 1, "Jr developer", 1 });

            migrationBuilder.InsertData(
                table: "UserTeam",
                columns: new[] { "Id", "CorporateId", "IndividualId", "TeamId" },
                values: new object[] { 1, 1, null, 1 });

            migrationBuilder.InsertData(
                table: "UserTeamPermission",
                columns: new[] { "Id", "PermissionId", "UserTeamId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "UserTeamRole",
                columns: new[] { "Id", "RoleId", "UserTeamId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Corporate_CompanyId",
                table: "Corporate",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_TeamId",
                table: "Role",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_CompanyId",
                table: "Team",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeam_CorporateId",
                table: "UserTeam",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeam_IndividualId",
                table: "UserTeam",
                column: "IndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeam_TeamId",
                table: "UserTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamPermission_PermissionId",
                table: "UserTeamPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamPermission_UserTeamId",
                table: "UserTeamPermission",
                column: "UserTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamRole_RoleId",
                table: "UserTeamRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamRole_UserTeamId",
                table: "UserTeamRole",
                column: "UserTeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "RegisterOutbox");

            migrationBuilder.DropTable(
                name: "UserTeamPermission");

            migrationBuilder.DropTable(
                name: "UserTeamRole");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "UserTeam");

            migrationBuilder.DropTable(
                name: "Corporate");

            migrationBuilder.DropTable(
                name: "Individual");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");
        }
    }
}
