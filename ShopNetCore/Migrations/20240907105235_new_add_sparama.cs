using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopNetCore.Migrations
{
  /// <inheritdoc />
  public partial class new_add_sparama : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "sp_Aramas",
          columns: table => new
          {
            ID = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            KATEGORI = table.Column<int>(type: "int", nullable: false),
            URUN = table.Column<int>(type: "int", nullable: false),
            MARKA = table.Column<int>(type: "int", nullable: false),
            ARAMAISMI = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_sp_Aramas", x => x.ID);
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "sp_Aramas");
    }
  }
}
