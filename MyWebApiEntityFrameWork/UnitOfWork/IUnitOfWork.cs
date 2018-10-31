using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.EntityFramework.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
    }
}
