using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class dbwe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "5669930f-7b50-42b4-8d59-6f1b0fa2b499", null, "Admin", "ADMIN" },
                    { "daa1cb7b-88c4-41e6-bbe8-deac2951fb76", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5669930f-7b50-42b4-8d59-6f1b0fa2b499");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "daa1cb7b-88c4-41e6-bbe8-deac2951fb76");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6cb5eb3c-226f-4580-9c2f-7c3b40ccee6f", null, "Admin", "ADMIN" },
                    { "788abfc4-e5f7-4112-82fd-a8d37b5ac6f6", null, "User", "USER" }
                });
        }
    }
}
