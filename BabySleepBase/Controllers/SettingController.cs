using BabySleepBase.Models;
using SQLite;
using System.Threading.Tasks;

namespace BabySleepBase.Controllers
{
    public class SettingController
    {
        SQLiteAsyncConnection dbConnection;

        public SettingController(SQLiteAsyncConnection sqlConnection)
        {
            dbConnection = sqlConnection;
        }

        public Task<Setting> GetSettings()
        {
            return dbConnection.Table<Setting>().FirstOrDefaultAsync();
        }

        public Task<int> SaveSettings(Setting settings)
        {
            return dbConnection.UpdateAsync(settings);
        }

        public Task<int> CreateSettings(Setting settings)
        {
            return dbConnection.InsertAsync(settings);
        }
    }
}
