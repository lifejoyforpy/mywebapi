using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.EntityFramework.UnitOfWork
{

    //负责整个生命周期一个dbcontext保证事物的一致性。
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private TContext _context;
        public UnitOfWork(TContext context)
        {
            _context = context;
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
