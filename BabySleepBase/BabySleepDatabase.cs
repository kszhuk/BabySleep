using BabySleepBase.Controllers;
using BabySleepBase.Models;
using SQLite;

namespace BabySleepBase
{
    /// <summary>
    /// Initialize connection
    /// Create db & tables if needed
    /// </summary>
    public class BabySleepDatabase
    {
        readonly SQLiteAsyncConnection dbConnection;
        ChildController childController;
        public SQLiteAsyncConnection DbConnection
        {
            get => dbConnection;
        }
        public BabySleepDatabase(string dbPath)
        {
            dbConnection = new SQLiteAsyncConnection(dbPath);
            InitDatabase();
        }

        private void InitDatabase()
        {
            //_database.DropTableAsync<Child>().Wait();
            //_database.DropTableAsync<Setting>().Wait();
            CreateTableAsync();
        }

        private void CreateTableAsync()
        {
            dbConnection.CreateTableAsync<Setting>().Wait();
            dbConnection.CreateTableAsync<Child>().Wait();
        }
    }
}
