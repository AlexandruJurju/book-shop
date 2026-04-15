using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.AspNetCore.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
