using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Domain.Test
{
    public class Order : Entity<long>
    {
        public string OrderNo { get; set; }
        public double Price { get; set; }
    }

  
}
