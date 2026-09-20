using System.Data.Common;
using BuildingBlocks.Application.Abstractions.Data;
using Npgsql;

namespace BuildingBlocks.Infrastructure.Data.Postgres;

public sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
