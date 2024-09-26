using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class dbwebs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e4ea800-5d9c-4838-81dc-2044a82e8abd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "651859c0-2296-4ce9-8703-67ee9d18b637");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6cb5eb3c-226f-4580-9c2f-7c3b40ccee6f", null, "Admin", "ADMIN" },
                    { "788abfc4-e5f7-4112-82fd-a8d37b5ac6f6", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cb5eb3c-226f-4580-9c2f-7c3b40ccee6f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "788abfc4-e5f7-4112-82fd-a8d37b5ac6f6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e4ea800-5d9c-4838-81dc-2044a82e8abd", null, "User", "USER" },
                    { "651859c0-2296-4ce9-8703-67ee9d18b637", null, "Admin", "ADMIN" }
                });
        }
    }
}
