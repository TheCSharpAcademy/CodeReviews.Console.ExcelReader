using System.Data;
using System.Data.SqlClient;
using ExcelReader.UgniusFalze.Models;

namespace ExcelReader.UgniusFalze.Services;

public class Driver
{
    public SqlConnection SqlConn { get; }

    public Driver(string connectionString)
    {
        SqlConn = new SqlConnection(connectionString);
    }

    private void SimpleTSql(string tsql)
    {
        var command = SqlConn.CreateCommand();
        command.CommandText = tsql;
        SqlConn.Open();
        command.Prepare();
        command.ExecuteNonQuery();
        SqlConn.Close();
    }

    public void DropDatabase()
    {
        string tsql = "IF EXISTS (SELECT name FROM sys.databases WHERE (name = \'ExcelReader\'))" +
                      " BEGIN" +
                      "     ALTER DATABASE ExcelReader SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                      "     DROP DATABASE ExcelReader;" +
                      " END;";
        SimpleTSql(tsql);
    }

    public void CreateDatabase()
    {
        SimpleTSql("CREATE DATABASE ExcelReader");
    }

    public void CreateTable()
    {
        string tsql = "CREATE TABLE ExcelReader.dbo.Planes ( " +
                      "PlaneId INT IDENTITY CONSTRAINT PlaneId PRIMARY KEY," +
                      "Manufacturer nvarchar(MAX) NOT NULL," +
                      "Model nvarchar(MAX) NOT NULL," +
                      "Type nvarchar(MAX) NOT NULL," +
                      "MaxSpeed INTEGER NOT NULL," +
                      "Capacity INTEGER NOT NULL," +
                      "FirstFlightDate SMALLDATETIME NOT NULL )";
        SimpleTSql(tsql);
    }

    public void InsertPlane(Plane plane)
    {
        var command = SqlConn.CreateCommand();
        command.CommandText =
            "INSERT INTO ExcelReader.dbo.Planes(Manufacturer, Model, Type, MaxSpeed, Capacity, FirstFlightDate)" +
            "VALUES (@manufacturer, @model, @type, @maxSpeed, @capacity, @firstFlightDate)";
        command.Parameters.Add("@manufacturer", SqlDbType.NVarChar, 255).Value = plane.Manufacturer;
        command.Parameters.Add("@model", SqlDbType.NVarChar, 255).Value = plane.Model;
        command.Parameters.Add("@type", SqlDbType.NVarChar, 255).Value = plane.Type;
        command.Parameters.Add("@maxSpeed", SqlDbType.Int).Value = plane.MaxSpeed;
        command.Parameters.Add("@capacity", SqlDbType.Int).Value = plane.Capacity;
        command.Parameters.Add("@firstFlightDate", SqlDbType.SmallDateTime).Value = plane.FirstFlightDate;
        SqlConn.Open();
        command.Prepare();
        command.ExecuteNonQuery();
        SqlConn.Close();
    }

    public List<Plane> GetPlanes()
    {
        var command = SqlConn.CreateCommand();
        command.CommandText = "SELECT Manufacturer, Model, Type, MaxSpeed, Capacity, FirstFlightDate " +
                              "FROM ExcelReader.dbo.Planes ";
        var planes = new List<Plane>();
        SqlConn.Open();
        command.Prepare();
        using (var dataReader = command.ExecuteReader())
        {
            while (dataReader.Read())
            {
                planes.Add(new Plane(dataReader.GetString(0), dataReader.GetString(1), 
                    dataReader.GetString(2), dataReader.GetInt32(3), dataReader.GetInt32(4),
                    dataReader.GetDateTime(5)));
            }
        }

        return planes;
    }


}