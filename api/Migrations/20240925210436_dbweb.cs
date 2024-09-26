using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class dbweb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8de7370e-ac61-406a-9b62-63fa2991d283");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7a0877b-d640-426f-b2e8-0106f1f9ba7a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e4ea800-5d9c-4838-81dc-2044a82e8abd", null, "User", "USER" },
                    { "651859c0-2296-4ce9-8703-67ee9d18b637", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "8de7370e-ac61-406a-9b62-63fa2991d283", null, "Admin", "ADMIN" },
                    { "b7a0877b-d640-426f-b2e8-0106f1f9ba7a", null, "User", "USER" }
                });
        }
    }
}
