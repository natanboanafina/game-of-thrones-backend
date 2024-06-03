using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameOfThrones.Migrations
{
    /// <inheritdoc />
    public partial class CharactersHousesAndData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Characters");

            migrationBuilder.AddColumn<int>(
                name: "DataId",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Datas",
                columns: table => new
                {
                    DataId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datas", x => x.DataId);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    HouseId = table.Column<int>(type: "integer", nullable: false),
                    HouseName = table.Column<string>(type: "text", nullable: false),
                    DataId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.HouseId);
                    table.ForeignKey(
                        name: "FK_Houses_Datas_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Datas",
                        principalColumn: "DataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_DataId",
                table: "Characters",
                column: "DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Datas_DataId",
                table: "Characters",
                column: "DataId",
                principalTable: "Datas",
                principalColumn: "DataId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Datas_DataId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "Datas");

            migrationBuilder.DropIndex(
                name: "IX_Characters_DataId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DataId",
                table: "Characters");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
