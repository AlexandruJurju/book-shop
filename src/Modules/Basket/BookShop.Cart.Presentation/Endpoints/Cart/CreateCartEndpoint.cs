using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BookShop.Cart.Presentation.Endpoints.Cart;

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("cart", Handle)
            .WithTags(Tags.Cart);
    }

    private static async Task<IResult> Handle()
    {
        return Results.Ok();
    }
}
