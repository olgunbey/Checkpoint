using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checkpoint.IdentityServer.SagaOrchestration.Migrations
{
    /// <inheritdoc />
    public partial class InstanceUpdateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "IdentityServerStateInstance");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "IdentityServerStateInstance");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "IdentityServerStateInstance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "IdentityServerStateInstance",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "IdentityServerStateInstance",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "IdentityServerStateInstance",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
