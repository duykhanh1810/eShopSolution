using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
	public partial class SeedData : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "SeoAlias",
				table: "Products");

			migrationBuilder.InsertData(
				table: "AppConfigs",
				columns: new[] { "Key", "Value" },
				values: new object[,]
				{
					{ "HomeTitle", "This is the home page of eShopSolution" },
					{ "HomeKeyword", "This is keyword of eShopSolution" },
					{ "HomeDescription", "This is description of eShopSolution" }
				});

			migrationBuilder.InsertData(
				table: "Categories",
				columns: new[] { "Id", "IsShowOnHome", "ParentId", "SortOrder", "Status" },
				values: new object[,]
				{
					{ 1, true, null, 1, 1 },
					{ 2, true, null, 2, 1 }
				});

			migrationBuilder.InsertData(
				table: "Languages",
				columns: new[] { "Id", "IsDefault", "Name" },
				values: new object[,]
				{
					{ "vi", true, "Tiếng Việt" },
					{ "en", false, "English" }
				});

			migrationBuilder.InsertData(
				table: "Products",
				columns: new[] { "Id", "DateCreated", "OriginalPrice", "Price" },
				values: new object[] { 1, new DateTime(2023, 2, 27, 18, 55, 34, 260, DateTimeKind.Local).AddTicks(6133), 100000m, 200000m });

			migrationBuilder.InsertData(
				table: "CategoryTranslations",
				columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SeoAlias", "SeoDescription", "SeoTitle" },
				values: new object[,]
				{
					{ 1, 1, "vi", "Áo Nam", "ao-nam", "Sản phẩm áo thời trang nam", "Sản phẩm áo thời trang nam" },
					{ 3, 2, "vi", "Áo Nữ", "ao-nu", "Sản phẩm áo thời trang nữ", "Sản phẩm áo thời trang nữ" },
					{ 2, 1, "en", "Men Shirt", "men-shirt", "The shirt product for men", "The shirt product for men" },
					{ 4, 2, "en", "Women Shirt", "women-shirt", "The shirt product for women", "The shirt product for women" }
				});

			migrationBuilder.InsertData(
				table: "ProductInCategories",
				columns: new[] { "CategoryId", "ProductId" },
				values: new object[] { 1, 1 });

			migrationBuilder.InsertData(
				table: "ProductTranslations",
				columns: new[] { "Id", "Description", "Details", "LanguageId", "Name", "ProductId", "SeoAlias", "SeoDescription", "SeoTitle" },
				values: new object[,]
				{
					{ 1, "Áo Sơ Mi Nam trắng Việt Tiến", "Áo Sơ Mi Nam trắng Việt Tiến", "vi", "Áo Sơ Mi Nam trắng Việt Tiến", 1, "ao-so-mi-nam-trang-viet-tien", "Áo Sơ Mi Nam trắng Việt Tiến", "Áo Sơ Mi Nam trắng Việt Tiến" },
					{ 2, "Viet Tien Men T-Shirt", "Viet Tien Men T-Shirt", "en", "Viet Tien Men T-Shirt", 1, "viet-tien-men-tshirt", "Viet Tien Men T-Shirt", "Viet Tien Men T-Shirt" }
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AppConfigs",
				keyColumn: "Key",
				keyValue: "HomeDescription");

			migrationBuilder.DeleteData(
				table: "AppConfigs",
				keyColumn: "Key",
				keyValue: "HomeKeyword");

			migrationBuilder.DeleteData(
				table: "AppConfigs",
				keyColumn: "Key",
				keyValue: "HomeTitle");

			migrationBuilder.DeleteData(
				table: "CategoryTranslations",
				keyColumn: "Id",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "CategoryTranslations",
				keyColumn: "Id",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "CategoryTranslations",
				keyColumn: "Id",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "CategoryTranslations",
				keyColumn: "Id",
				keyValue: 4);

			migrationBuilder.DeleteData(
				table: "ProductInCategories",
				keyColumns: new[] { "CategoryId", "ProductId" },
				keyValues: new object[] { 1, 1 });

			migrationBuilder.DeleteData(
				table: "ProductTranslations",
				keyColumn: "Id",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "ProductTranslations",
				keyColumn: "Id",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Categories",
				keyColumn: "Id",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Categories",
				keyColumn: "Id",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Languages",
				keyColumn: "Id",
				keyValue: "en");

			migrationBuilder.DeleteData(
				table: "Languages",
				keyColumn: "Id",
				keyValue: "vi");

			migrationBuilder.DeleteData(
				table: "Products",
				keyColumn: "Id",
				keyValue: 1);

			migrationBuilder.AddColumn<string>(
				name: "SeoAlias",
				table: "Products",
				type: "nvarchar(max)",
				nullable: true);
		}
	}
}