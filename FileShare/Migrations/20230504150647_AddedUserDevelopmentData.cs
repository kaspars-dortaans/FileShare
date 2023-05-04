using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileShare.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserDevelopmentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "Admin", "Test", "$2a$11$Mprpo/PxbtIDUx/UIhX89e1L2GgPEiu0w4yZ5KDor1ECPaZRE789O", 0, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
