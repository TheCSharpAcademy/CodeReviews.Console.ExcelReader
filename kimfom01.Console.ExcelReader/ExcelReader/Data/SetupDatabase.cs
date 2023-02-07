using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace ExcelReader.Data;

public class SetupDatabase : ISetupDatabase
{
    private readonly IConfiguration _config;

    public SetupDatabase(IConfiguration config)
    {
        _config = config;
    }

    public void Setup()
    {
        var databaseConnection = _config.GetConnectionString("ServerConnection");
        var file = new FileInfo(_config.GetSection("SetupScript").Value!);
        var script = file.OpenText().ReadToEnd();

        using var connection = new SqlConnection(databaseConnection);

        var serverConnection = new ServerConnection(connection);

        var server = new Server(serverConnection);

        server.ConnectionContext.ExecuteNonQuery(script);
    }
}