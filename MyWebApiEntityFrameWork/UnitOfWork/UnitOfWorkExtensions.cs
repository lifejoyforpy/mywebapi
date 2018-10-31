using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.EntityFramework.UnitOfWork
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWork<TContext>( this IServiceCollection services) where TContext:DbContext
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        }
    }
}
