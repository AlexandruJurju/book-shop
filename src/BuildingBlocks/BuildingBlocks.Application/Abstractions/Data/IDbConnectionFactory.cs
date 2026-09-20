using System.Data.Common;

namespace BuildingBlocks.Application.Abstractions.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
