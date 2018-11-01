using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebApi.Domain.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.EntityFramework.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(X => X.Name).IsRequired().HasMaxLength(50);
            builder.Property(X => X.Description).IsRequired().HasMaxLength(200);
        }
    }
}
