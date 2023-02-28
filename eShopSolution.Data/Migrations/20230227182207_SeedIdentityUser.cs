using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class SeedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("a4755a62-dcba-47ed-82bf-d8badb2c58d6"), "9844b927-79f0-4810-8bd6-e1aeb76e35c9", "Administrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("a4755a62-dcba-47ed-82bf-d8badb2c58d6"), new Guid("39f19ea5-0d36-440a-9074-542453d01eb4") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("39f19ea5-0d36-440a-9074-542453d01eb4"), 0, "1f86f1b9-d59d-4605-8c8c-f1b10c05084a", new DateTime(2002, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "duykhanh18102002@gmail.com", true, "Khanh", "Dinh", false, null, "duykhanh18102002@gmail.com", "admin", "AQAAAAEAACcQAAAAEJy8Bfr3idNGQgLHgC6yo1DrunsMi/ebKYOTWTqfYI7N1/WoQ9cLNTaNrpgeTL1lVw==", null, false, "", false, "admin" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 2, 28, 1, 22, 6, 97, DateTimeKind.Local).AddTicks(3076));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a4755a62-dcba-47ed-82bf-d8badb2c58d6"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a4755a62-dcba-47ed-82bf-d8badb2c58d6"), new Guid("39f19ea5-0d36-440a-9074-542453d01eb4") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("39f19ea5-0d36-440a-9074-542453d01eb4"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 2, 28, 1, 5, 21, 738, DateTimeKind.Local).AddTicks(8922));
        }
    }
}
