using Npgsql;

namespace ABCinema_WPF.Data;

public static class DbConnectionFactory
{
    private const string IpAdress = "localhost";
    private const string Port = "5433";
    private const string DbName = "cinema";
    private const string Username = "admin";
    private const string Password = "secret123";
    
    private const string ConnectionString = $"Host={IpAdress};Port={Port};Database={DbName};Username={Username};Password={Password}";

    public static NpgsqlConnection CreateConnection() {
        return new NpgsqlConnection(ConnectionString);
    }
}