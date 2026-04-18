using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berryfy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactor_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartCoupons_AspNetUsers_UserId",
                table: "CartCoupons");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ShoppingCarts");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CartCoupons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "CartCoupons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartCoupons_AspNetUsers_UserId",
                table: "CartCoupons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartCoupons_AspNetUsers_UserId",
                table: "CartCoupons");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "CartCoupons");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ShoppingCarts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CartCoupons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartCoupons_AspNetUsers_UserId",
                table: "CartCoupons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
