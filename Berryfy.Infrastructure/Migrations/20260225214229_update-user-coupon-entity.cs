using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berryfy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateusercouponentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "UserCoupons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedAt",
                table: "UserCoupons",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "UserCoupons");

            migrationBuilder.DropColumn(
                name: "UsedAt",
                table: "UserCoupons");
        }
    }
}
