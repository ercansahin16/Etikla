using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopNetCore.Migrations
{
  /// <inheritdoc />
  public partial class add_active_column : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
          name: "Active",
          table: "Emails",
          type: "bit",
          nullable: false,
          defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "Active",
          table: "Emails");
    }
  }
}
