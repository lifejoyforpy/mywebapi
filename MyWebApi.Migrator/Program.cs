using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using MyWebApi.EntityFramework;

namespace MyWebApi.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entity Framework Core Migrate Start !");
            Console.WriteLine("Get Pending Migrations...");

            using (var db = new MyContext(new DbContextOptionsBuilder<MyContext>().UseSqlServer("Server=localhost; Database=MywebApi; Trusted_Connection=True;").Options))
            {
                //获取所有待迁移
                Console.WriteLine($"Pending Migrations：\n{string.Join('\n', db.Database.GetPendingMigrations().ToArray())}");

                Console.WriteLine("Do you want to continue?(Y/N)");

                if (Console.ReadLine().Trim().ToLower() == "n")
                {
                    return;
                }

                Console.WriteLine("Migrating...");

                try
                {

                    //执行迁移
                    db.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }

            Console.WriteLine("Entity Framework Core Migrate Complete !");
            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }
    }
}
