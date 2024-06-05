using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdefixKitapLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdefixId",
                table: "Kitaplar",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdefixId",
                table: "Kitaplar");
        }
    }
}
