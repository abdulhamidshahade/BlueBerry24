using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueBerry24.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_couponCode_property_from_userCoupon_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "UserCoupons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "UserCoupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
