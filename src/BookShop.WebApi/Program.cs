using System.Reflection;
using BookShop.ServiceDefaults;
using BookShop.WebApi;
using BookShop.WebApi.Extensions;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

List<Assembly> moduleAssemblies =
[
    BookShop.Catalog.Application.AssemblyReference.Assembly
];

builder.AddModules(moduleAssemblies);

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

await app.RunAsync();
