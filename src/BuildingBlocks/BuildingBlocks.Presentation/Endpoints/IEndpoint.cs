using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
