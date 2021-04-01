using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace BabySleep.EfData.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlite(@"Server=.\;Database=db;Trusted_Connection=True;",
                x => x.MigrationsAssembly("BabySleep.EfData.Migrations"));
            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
