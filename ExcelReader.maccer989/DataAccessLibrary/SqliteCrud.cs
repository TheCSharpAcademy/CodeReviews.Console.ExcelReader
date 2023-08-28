using ExcelReader;
using System.Data.SQLite;
using System.Collections.Generic;

namespace DataAccessLibrary
{

    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new SqliteDataAccess();

        public SqliteCrud(string connectionString)
        {
            _connectionString = connectionString;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS People ( 
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT,
                LastName TEXT)";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void CreateContact(List<PersonModel> person)
        {            
            string sql = "insert into People (FirstName, LastName) values (@FirstName, @LastName);";

            foreach (var p in person)
            {
                db.SaveData(sql, new { p.FirstName, p.LastName }, _connectionString);
            }
        }

        public List<PersonModel> GetAllSessions()
        {
            string sql = "select Id, FirstName, LastName from People";
            return db.LoadData<PersonModel, dynamic>(sql, new { }, _connectionString);
        }
    }
}