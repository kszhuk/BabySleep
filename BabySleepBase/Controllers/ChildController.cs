using BabySleepBase.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabySleepBase.Controllers
{
    public class ChildController
    {
        SQLiteAsyncConnection dbConnection;

        public ChildController(SQLiteAsyncConnection sqlConnection)
        {
            dbConnection = sqlConnection;
        }

        public Task<List<Child>> GetChildren()
        {
            return dbConnection.Table<Child>().ToListAsync();
        }

        public Task<Child> GetChild(Guid childGuid)
        {
            return dbConnection.Table<Child>().Where(i => i.ChildGuid == childGuid)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveChild(Child child)
        {
            if (child.ChildGuid != Guid.Empty)
            {
                return dbConnection.UpdateAsync(child);
            }
            else
            {
                child.ChildGuid = Guid.NewGuid();
                return dbConnection.InsertAsync(child);
            }
        }

        public Task<int> DeleteChild(Child child)
        {
            return dbConnection.DeleteAsync(child);
        }
    }
}
