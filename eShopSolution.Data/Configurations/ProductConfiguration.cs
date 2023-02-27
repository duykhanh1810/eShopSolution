using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.OriginalPrice).IsRequired();
            builder.Property(p => p.Stock).IsRequired().HasDefaultValue(0);
            builder.Property(p => p.ViewCount).IsRequired().HasDefaultValue(0);

        }
    }
}
