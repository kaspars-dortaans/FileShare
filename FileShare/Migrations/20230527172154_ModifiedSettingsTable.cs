using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileShare.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Settings",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Settings",
                newName: "Name");
        }
    }
}
