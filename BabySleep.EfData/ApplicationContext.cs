using BabySleep.Common.Interfaces;
using BabySleep.EfData.Interfaces;
using BabySleep.EfData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.EfData
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        private readonly string _databasePath;
        private readonly bool _testingMode = false;

        public DbSet<Child> Children { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Sleep> Sleeps { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options, bool testingMode) : base(options) 
        {
            _testingMode = testingMode;
        }

        public ApplicationContext(string databasePath)
        {
            _databasePath = Path.Combine(databasePath, DatabaseName);

            SQLitePCL.Batteries_V2.Init();

            //this.Database.EnsureDeleted();
            this.Database.Migrate();
        }

        public ApplicationContext(IAppConfig appConfig) : this(appConfig.GetConnectionString()) { }

        private const string DatabaseName = "BabySleep.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!_testingMode)
            {
                optionsBuilder.UseSqlite($"Filename={_databasePath}");
            }
        }
    }
}
