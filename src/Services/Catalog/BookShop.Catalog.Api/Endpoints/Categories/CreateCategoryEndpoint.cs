using Ardalis.Result;
using BookShop.Catalog.Application.Categories.CreateCategory;
using BuildingBlocks.AspNetCore.Endpoints;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BookShop.Catalog.Api.Endpoints.Categories;

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", Handle)
            .WithTags(Tags.Categories);
    }

    private static async Task<IResult> Handle(Request request, ISender sender)
    {
        Result<Guid> result = await sender.Send(new CreateCategoryCommand(request.Name));

        return Results.Ok(result);
    }

    internal sealed class Request
    {
        public string Name { get; init; }
    }
}
