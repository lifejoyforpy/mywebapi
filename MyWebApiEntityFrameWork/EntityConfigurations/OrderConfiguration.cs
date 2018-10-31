using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.EntityFramework.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderNo).IsRequired();

        }
    }
}
