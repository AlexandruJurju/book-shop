using System.Reflection;
using BookShop.Catalog.Api;
using BookShop.ServiceDefaults;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using BuildingBlocks.Api.Endpoints;
using BuildingBlocks.Api.ExceptionHandler;
using BuildingBlocks.Api.Scalar;
using Quartz;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

Assembly[] moduleApplicationAssemblies = [BookShop.Catalog.Application.AssemblyMarker.Assembly];

builder.AddApplication(moduleApplicationAssemblies);
builder.AddPresentation();
builder.AddInfrastructure([]);

builder.AddCatalogModule();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.MapCustomScalar(builder.Configuration);
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapQuartzHttpApi().AllowAnonymous();
app.MapQuartzDashboard().AllowAnonymous();

app.MapEndpoints();

await app.RunAsync();
