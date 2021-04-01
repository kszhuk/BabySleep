using BabySleep.EfData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.EfData.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<Child> Children { get; set; }
        DbSet<Setting> Settings { get; set; }
        int SaveChanges();
    }
}
