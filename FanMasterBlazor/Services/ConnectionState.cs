using Microsoft.Data.SqlClient;

namespace FanMasterBlazor.Services;

public class ConnectionState
{
    public string Server { get; set; } = "fanserver.database.windows.net";
    public string Database { get; set; } = "FanMaster";
    public string? UserName { get; set; }
    public string? Password { get; set; }

    public bool IsConnected => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);

    public string BuildConnectionString()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("Debe iniciar sesión antes de conectarse.");
        }

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = Server,
            InitialCatalog = Database,
            UserID = UserName,
            Password = Password,
            Encrypt = true,
            TrustServerCertificate = false,
            ConnectTimeout = 30
        };

        return builder.ConnectionString;
    }

    public void Disconnect()
    {
        UserName = null;
        Password = null;
    }
}
