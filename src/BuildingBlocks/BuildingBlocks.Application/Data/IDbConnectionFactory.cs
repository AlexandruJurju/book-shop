using System.Data.Common;

namespace BuildingBlocks.Application.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
