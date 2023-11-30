using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Awacash.Infrastructure.Migrations
{
    public partial class DB_00014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Customers");
        }
    }
}
