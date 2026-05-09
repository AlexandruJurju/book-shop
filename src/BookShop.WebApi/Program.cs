using System.Reflection;
using BookShop.Cart.Presentation;
using BookShop.Catalog;
using BookShop.ServiceDefaults;
using BookShop.Users.Presentation;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using BuildingBlocks.AspNetCore.Endpoints;
using BuildingBlocks.AspNetCore.ExceptionHandler;
using BuildingBlocks.AspNetCore.Scalar;
using TickerQ.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

Assembly[] moduleApplicationAssemblies =
[
    BookShop.Users.Application.AssemblyReference.Assembly,
    BookShop.Cart.Application.AssemblyReference.Assembly
];

builder.Services.AddPresentation();
builder.Services.AddApplication(moduleApplicationAssemblies);
builder.AddInfrastructure(
[
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
