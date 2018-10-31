using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Domain.Test
{
    public class Material : Entity<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            //级联删除
            builder.HasOne(x => x.Product).WithMany(x => x.Materials).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }

}
