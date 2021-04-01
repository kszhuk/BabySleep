using BabySleep.EfData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Tests.Helpers
{
    [Obsolete]
    public class DbContextFactory : IDisposable
    {
        private SqliteConnection _connection;

        private DbContextOptions<ApplicationContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(_connection).Options;
        }

        public ApplicationContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();
                using (var context = new ApplicationContext(options, true))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new ApplicationContext(CreateOptions());
        }

        //    using (var factory = new DbContextFactory())
        //{
        //    using (var context = factory.CreateContext())
        //    {
        //        Assert.Equal(0, context.Children.Count());
        //        var repository = new ChildRepository(context);
        //        repository.Add(child);
        //        Assert.Equal(1, context.Children.Count());
        //    }
        //}

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
