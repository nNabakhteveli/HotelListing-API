using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9277bd7c-47e0-4f8e-88fb-05e6081b680b", "5a5ce5eb-1e96-450c-ac6e-569265e576cc", "User", "USER" },
                    { "b6832497-3f9c-4146-aef0-62c514ae6133", "497e414b-9f7d-485d-bc06-589c7e6e96bb", "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9277bd7c-47e0-4f8e-88fb-05e6081b680b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6832497-3f9c-4146-aef0-62c514ae6133");
        }
    }
}
