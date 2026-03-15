using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Chassis.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}