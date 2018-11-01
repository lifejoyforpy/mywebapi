using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.Extensions
{
   public static class CollectionExtensions
    {
        public static void AddIfNot<T>(this ICollection<T> list, T entity) 
        {
            if (list == null)
            {

                throw new ArgumentNullException();
            }
            if (!list.Contains(entity))
            {
                list.Add(entity);
            }
        }
    }
}
