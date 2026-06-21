using BuildingBlocks.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BookShop.Cart.Presentation.Endpoints.Cart;

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
