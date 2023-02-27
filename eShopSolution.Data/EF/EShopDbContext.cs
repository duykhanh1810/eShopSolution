﻿using eShopSolution.Data.Configurations;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.EF
{
    public class EShopDbContext : DbContext
    {
        
        public EShopDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CategoryTranslation> CategoriesTranslation { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        //end 7

        // Phương thức này thi hành khi EnsureCreatedAsync chạy, tại đây gọi các Fluent API mong muốn 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //8. Add config ở đây
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());

            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());

            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());

            modelBuilder.ApplyConfiguration(new CartConfiguration());

            modelBuilder.ApplyConfiguration(new ContactConfiguration());

            modelBuilder.ApplyConfiguration(new LanguageConfiguration());

            modelBuilder.ApplyConfiguration(new PromotionConfiguration());

            modelBuilder.ApplyConfiguration(new TransactionConfiguration());

            modelBuilder.ApplyConfiguration(new CategoryTranslationConfiguration());

            modelBuilder.ApplyConfiguration(new ProductTranslationConfiguration());
        }
    }
}