using BabySleepBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleepBase.EFCoreApp
{
    public class ApplicationContext : DbContext
    {
        private string _databasePath;

        public DbSet<Child> Children { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        private const string DatabaseName = "myItems.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }

    }
}
