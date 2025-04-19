using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checkpoint.IdentityServer.Migrations.IdentityServerOutboxDb
{
    /// <inheritdoc />
    public partial class RegisterOutboxOccurredColumnDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occurred",
                table: "RegisterOutbox");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Occurred",
                table: "RegisterOutbox",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
