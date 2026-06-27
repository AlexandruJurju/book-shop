using System.Reflection;
using BookShop.Cart;
using BookShop.Catalog;
using BookShop.ServiceDefaults;
using BookShop.Users;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ExceptionHandler;
using BuildingBlocks.Presentation.Scalar;
using TickerQ.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddPresentation();
builder.AddInfrastructure(
[
    BookShop.Cart.DependencyInjection.ConfigureConsumers
]);

builder.AddUsersModule();
builder.AddCatalogModule();
builder.AddCartModule();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

app.MapCustomScalar(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.UseTickerQ();

app.MapEndpoints();

await app.RunAsync();
