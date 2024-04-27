using GenApp.WebApi.Extensions;
using GenApp.WebApi.Middlewares;
using GenApp.DomainServices;
using GenApp.Templates.Parser;
using GenApp.Parsers.Csharp;
using GenApp.Parsers.Sql;
using GenApp.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogLogging(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration);
builder.Services.AddTemplateParser();
builder.Services.AddDomainServices();
builder.Services.AddCSharpParsers();
builder.Services.AddSqlParsers();

var app = builder.Build();

app.UseErrorHandling();

app.UseHttpsRedirection();

app.UseCors(ApiConstants.CorsPolicy);

app.UseRouting();

app.UseRequestLogger();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
