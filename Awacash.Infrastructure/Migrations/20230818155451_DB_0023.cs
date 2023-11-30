using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Awacash.Infrastructure.Migrations
{
    public partial class DB_0023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "SavingsConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeductionAccount",
                table: "Savings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SavingAccount",
                table: "Savings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "SavingsConfigurations");

            migrationBuilder.DropColumn(
                name: "DeductionAccount",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "SavingAccount",
                table: "Savings");
        }
    }
}
