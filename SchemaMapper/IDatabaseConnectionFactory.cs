using Npgsql;

namespace SchemaMapper;

public interface IDatabaseConnectionFactory
{
    NpgsqlConnection GetConnection();
}