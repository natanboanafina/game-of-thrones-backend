using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfThrones.Migrations
{
    /// <inheritdoc />
    public partial class CharactersAndData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Datas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "House",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Datas");

            migrationBuilder.DropColumn(
                name: "House",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Characters");
        }
    }
}
