using System.Reflection;
using BookShop.Cart.Presentation;
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

Assembly[] moduleApplicationAssemblies =
[
    BookShop.Users.AssemblyMarker.Assembly,
    BookShop.Cart.Application.AssemblyReference.Assembly
];

builder.Services.AddPresentation();
builder.Services.AddApplication(moduleApplicationAssemblies);
builder.AddInfrastructure(
[
    BookShop.Cart.Presentation.DependencyInjection.ConfigureConsumers
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
