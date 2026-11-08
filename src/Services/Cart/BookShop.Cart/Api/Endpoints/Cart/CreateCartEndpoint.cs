using BuildingBlocks.Api.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BookShop.Cart.Api.Endpoints.Cart;

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("cart", async (CancellationToken cancellationToken) =>
            {
                return Results.Ok();
            })
            .WithTags(Tags.Cart);
    }
}
