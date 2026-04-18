using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berryfy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactor_db_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
