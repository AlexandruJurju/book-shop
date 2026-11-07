using BuildingBlocks.Api.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BookShop.Catalog.Api.Endpoints;

public sealed class GetBooks : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("books", async (
                CancellationToken cancellationToken
            ) =>
            {
            })
            .WithTags(Tags.Books)
            .RequireAuthorization();
    }
}
