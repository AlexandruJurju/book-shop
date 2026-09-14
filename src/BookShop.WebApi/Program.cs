using System.Reflection;
using BookShop.Cart.Api;
using BookShop.ServiceDefaults;
using BookShop.Users.Api;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using BuildingBlocks.Api.Endpoints;
using BuildingBlocks.Api.ExceptionHandler;
using BuildingBlocks.Api.Scalar;
using TickerQ.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

Assembly[] moduleApplicationAssemblies =
[
    BookShop.Users.Application.AssemblyMarker.Assembly,
    BookShop.Cart.Application.AssemblyMarker.Assembly
];


builder.AddApplication(moduleApplicationAssemblies);
builder.AddPresentation();
builder.AddInfrastructure(
[
    BookShop.Cart.Infrastructure.DependencyInjection.ConfigureConsumers
]);

builder.AddUsersModule();
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
