using BuildingBlocks.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BookShop.Basket.Presentation.Endpoints.Basket;

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("basket", Handle)
            .WithTags(Tags.Basket);
    }

    private static async Task<IResult> Handle()
    {
        return Results.Ok();
    }
}
