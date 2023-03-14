using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class ChangeFileLengthType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a4755a62-dcba-47ed-82bf-d8badb2c58d6"),
                column: "ConcurrencyStamp",
                value: "c8a070ef-9f92-48bd-bf52-9f53682d60c4");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("39f19ea5-0d36-440a-9074-542453d01eb4"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c04778e3-95e9-4bfe-99ee-9f05b6371c27", "AQAAAAEAACcQAAAAEFLHxGk7rVB0DB8uVYUIBHvXi3b046cM/MdamAmuP5D01MsmjyeWRoLvap4Mg8JmnA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 3, 9, 1, 15, 22, 660, DateTimeKind.Local).AddTicks(964));
        }
    }
}
