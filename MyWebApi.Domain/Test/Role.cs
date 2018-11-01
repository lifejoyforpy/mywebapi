using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Domain.Test
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role:Entity<long>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
    }
   

}
