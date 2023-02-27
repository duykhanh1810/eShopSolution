using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTitle", Value = "This is the home page of eShopSolution" },
                new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of eShopSolution" },
                new AppConfig() { Key = "HomeDescription", Value = "This is description of eShopSolution" }
                );

            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en-US", Name = "English", IsDefault = false }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Status.Active,
                    
                },
                new Category()
                {
                    Id = 2,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 2,
                    Status = Status.Active,
                }
                );

            modelBuilder.Entity<CategoryTranslation>().HasData(                    
                        new CategoryTranslation(){
                            Id = 1,
                            CategoryId =1,
                            Name="Áo Nam",
                            LanguageId="vi-VN",
                            SeoAlias="ao-nam",
                            SeoDescription="Sản phẩm áo thời trang nam",
                            SeoTitle="Sản phẩm áo thời trang nam"
                        },
                        new CategoryTranslation(){
                            Id = 2,
                            CategoryId = 1,
                            Name="Men Shirt",
                            LanguageId="en-US",
                            SeoAlias="men-shirt",
                            SeoDescription="The shirt product for men",
                            SeoTitle="The shirt product for men"
                        },
                        new CategoryTranslation()
                        {
                            Id = 3,
                            CategoryId = 2,
                            Name = "Áo Nữ",
                            LanguageId = "vi-VN",
                            SeoAlias = "ao-nu",
                            SeoDescription = "Sản phẩm áo thời trang nữ",
                            SeoTitle = "Sản phẩm áo thời trang nữ"
                        },
                        new CategoryTranslation()
                        {
                            Id = 4,
                            CategoryId = 2,
                            Name = "Women Shirt",
                            LanguageId = "en-US",
                            SeoAlias = "women-shirt",
                            SeoDescription = "The shirt product for women",
                            SeoTitle = "The shirt product for women"
                        }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 100000,
                    Price = 200000,
                    Stock = 0,
                    ViewCount = 0,
                }
                );

            modelBuilder.Entity<ProductTranslation>().HasData(
                    new ProductTranslation()
                    {
                        Id =1,
                        ProductId = 1,
                        Name = "Áo Sơ Mi Nam trắng Việt Tiến",
                        LanguageId = "vi-VN",
                        SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                        SeoDescription = "Áo Sơ Mi Nam trắng Việt Tiến",
                        SeoTitle = "Áo Sơ Mi Nam trắng Việt Tiến",
                        Details = "Áo Sơ Mi Nam trắng Việt Tiến",
                        Description = "Áo Sơ Mi Nam trắng Việt Tiến"
                    },
                    new ProductTranslation()
                    {
                        Id = 2,
                        ProductId = 1,
                        Name = "Viet Tien Men T-Shirt",
                        LanguageId = "en-US",
                        SeoAlias = "viet-tien-men-tshirt",
                        SeoDescription = "Viet Tien Men T-Shirt",
                        SeoTitle = "Viet Tien Men T-Shirt",
                        Details = "Viet Tien Men T-Shirt",
                        Description = "Viet Tien Men T-Shirt"
                    }
                );

            modelBuilder.Entity<ProductInCategory>().HasData(
                    new ProductInCategory() {ProductId = 1, CategoryId = 1 }
                );
        }
    }
}
