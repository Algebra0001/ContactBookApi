using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATA.API.Migrations
{
    public partial class finalMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75ce43e0-a5f9-4dd9-9e81-fa460c63d62f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acd8ccf6-f91a-42f2-8ee6-7ff5d6b696da");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0037adfd-ff75-4797-8e5f-c8105341f0d5", "2", "USER", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3754fc80-717f-4c55-b4fd-af69ea420dd0", "1", "ADMIN", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0037adfd-ff75-4797-8e5f-c8105341f0d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3754fc80-717f-4c55-b4fd-af69ea420dd0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "75ce43e0-a5f9-4dd9-9e81-fa460c63d62f", "2", "USER", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "acd8ccf6-f91a-42f2-8ee6-7ff5d6b696da", "1", "ADMIN", "ADMIN" });
        }
    }
}
