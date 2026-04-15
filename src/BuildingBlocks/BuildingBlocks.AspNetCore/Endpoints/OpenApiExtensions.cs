using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.AspNetCore.Endpoints;

public static class OpenApiExtensions
{
    public static void UseDefaultOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapGet("/", () => TypedResults.Redirect("openapi/v1.json"))
                .ExcludeFromDescription();
        }
    }
}
