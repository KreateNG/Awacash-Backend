using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Awacash.Infrastructure.Migrations
{
    public partial class DB_00005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitAccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationBankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationBankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditAccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
