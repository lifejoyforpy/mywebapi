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



}
