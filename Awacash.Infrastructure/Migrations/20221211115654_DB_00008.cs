using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Awacash.Infrastructure.Migrations
{
    public partial class DB_00008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiary_Customers_CustomerId1",
                table: "Beneficiary");

            migrationBuilder.DropIndex(
                name: "IX_Beneficiary_CustomerId1",
                table: "Beneficiary");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Beneficiary");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecordType",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Beneficiary",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BeneficaryType",
                table: "Beneficiary",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiary_CustomerId",
                table: "Beneficiary",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiary_Customers_CustomerId",
                table: "Beneficiary",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiary_Customers_CustomerId",
                table: "Beneficiary");

            migrationBuilder.DropIndex(
                name: "IX_Beneficiary_CustomerId",
                table: "Beneficiary");

            migrationBuilder.DropColumn(
                name: "RecordType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BeneficaryType",
                table: "Beneficiary");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                table: "Beneficiary",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "Beneficiary",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiary_CustomerId1",
                table: "Beneficiary",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiary_Customers_CustomerId1",
                table: "Beneficiary",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
