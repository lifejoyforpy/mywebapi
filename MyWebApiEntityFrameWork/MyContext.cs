
using Microsoft.EntityFrameworkCore;
using MyWebApi.Domain.Test;
using MyWebApi.EntityFramework.EntityConfigurations;


namespace MyWebApi.EntityFramework
{
   public  class MyContext:DbContext
    {  

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Material> Materials { get; set; }

        public virtual DbSet<Order> Orders { get; set; }
        public MyContext(DbContextOptions<MyContext> optionsBuilder) :base(optionsBuilder)
        {
           // Database.Migrate();
        }

   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}
