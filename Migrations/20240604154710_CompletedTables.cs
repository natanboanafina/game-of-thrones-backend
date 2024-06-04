using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfThrones.Migrations
{
    /// <inheritdoc />
    public partial class CompletedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Characters",
                newName: "Gender");

            migrationBuilder.AddColumn<string>(
                name: "Lord",
                table: "Houses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Houses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Born",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Culture",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "Titles",
                table: "Characters",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lord",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Born",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Culture",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Titles",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Characters",
                newName: "Title");
        }
    }
}
