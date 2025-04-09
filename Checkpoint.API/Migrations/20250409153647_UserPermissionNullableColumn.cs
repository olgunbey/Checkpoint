using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Checkpoint.API.Migrations
{
    /// <inheritdoc />
    public partial class UserPermissionNullableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_Corporate_CorporateId",
                table: "UserPermission");

            migrationBuilder.AlterColumn<int>(
                name: "CorporateId",
                table: "UserPermission",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_Corporate_CorporateId",
                table: "UserPermission",
                column: "CorporateId",
                principalTable: "Corporate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_Corporate_CorporateId",
                table: "UserPermission");

            migrationBuilder.AlterColumn<int>(
                name: "CorporateId",
                table: "UserPermission",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_Corporate_CorporateId",
                table: "UserPermission",
                column: "CorporateId",
                principalTable: "Corporate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
