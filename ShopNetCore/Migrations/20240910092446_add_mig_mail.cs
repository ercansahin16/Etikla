using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopNetCore.Migrations
{
  /// <inheritdoc />
  public partial class add_mig_mail : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Emails",
          columns: table => new
          {
            EmailID = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            MailAdres = table.Column<string>(type: "nvarchar(max)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Emails", x => x.EmailID);
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Emails");
    }
  }
}
