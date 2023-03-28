using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class UpdateLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a4755a62-dcba-47ed-82bf-d8badb2c58d6"),
                column: "ConcurrencyStamp",
                value: "e9abd6d1-ac38-4073-a721-ef9bcad310ea");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("39f19ea5-0d36-440a-9074-542453d01eb4"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2a1ec899-2c17-4ab3-8b1d-4cd9115f8836", "AQAAAAEAACcQAAAAENdhovbEwvRkkgg5a0qjQfNJfo02ycUOmDATEpxLhzPvU0Y6eiZbJP8CSZNVNqtrWg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 3, 28, 22, 37, 16, 397, DateTimeKind.Local).AddTicks(3311));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a4755a62-dcba-47ed-82bf-d8badb2c58d6"),
                column: "ConcurrencyStamp",
                value: "08b59681-65cc-4f18-b48c-417ca91ee713");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("39f19ea5-0d36-440a-9074-542453d01eb4"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "23e2b55e-9a1b-43ea-8331-4b996be85b82", "AQAAAAEAACcQAAAAEJn3d98MBtMKu3/9dWEh4iJUU1f59ZGjOgb4+tzvDyb8Ny8/Q5AfO7EspML/mzKdQQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 3, 10, 0, 27, 31, 50, DateTimeKind.Local).AddTicks(1527));
        }
    }
}
