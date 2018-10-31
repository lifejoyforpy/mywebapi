using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Application.Dtos.ProductDto
{  
    /// <summary>
    /// 创建时候的viewmodel
    /// </summary>
    public class ProductCreation
    {
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
