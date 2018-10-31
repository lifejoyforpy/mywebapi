
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace MyWebApi.Domain.Test
{
    public class Product:Entity<long>
    {

        public Product()
        {
            Materials = new Collection<Material>();
        }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public ICollection<Material> Materials { get; set; }
    }

   
}
