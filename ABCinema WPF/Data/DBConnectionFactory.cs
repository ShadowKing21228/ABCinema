using Npgsql;

namespace ABCinema_WPF.Data;

public static class DbConnectionFactory
{
    private const string _ipAdress = "localhost";
    private const string _port = "5433";
    private const string _dbName = "cinema";
    private const string _username = "admin";
    private const string _password = "secret123";
    
    private const string _connectionString = $"Host={_ipAdress};Port={_port};Database={_dbName};Username={_username};Password={_password}";

    public static NpgsqlConnection CreateConnection() {
        return new NpgsqlConnection(_connectionString);
    }
}