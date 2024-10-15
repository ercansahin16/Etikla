using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopNetCore.Migrations
{
    /// <inheritdoc />
    public partial class puanlama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OrtalamaPuan",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "OySayisi",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToplamPuan",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Puan",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrtalamaPuan",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OySayisi",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ToplamPuan",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Puan",
                table: "Comments");
        }
    }
}
