using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checkpoint.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class CorporateVerificationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Verification",
                table: "Corporate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "Corporate",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verification",
                table: "Corporate");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "Corporate");
        }
    }
}
