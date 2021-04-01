using BabySleep.EfData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Tests.Helpers
{
    public class ApplicationContextFactory
    {
        private readonly DbContextOptions<ApplicationContext> options;

        public ApplicationContextFactory()
        {
            options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite("Filename=Test.db")
                .Options;
        }

        public ApplicationContext CreateContext()
        {
            using (var context = new ApplicationContext(options, true))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return new ApplicationContext(options, true);
        }
    }
}
