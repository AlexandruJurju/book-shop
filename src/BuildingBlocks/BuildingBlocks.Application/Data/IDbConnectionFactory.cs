using System.Data.Common;
using System.Threading.Tasks;

namespace BuildingBlocks.Application.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
