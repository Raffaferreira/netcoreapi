using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdatingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Debito",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Credito",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountTobeWithdraw",
                table: "Debito",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountTobeCredited",
                table: "Credito",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AccountTobeWithdraw",
                table: "Debito");

            migrationBuilder.DropColumn(
                name: "AccountTobeCredited",
                table: "Credito");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Debito",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Credito",
                newName: "FirstName");
        }
    }
}
