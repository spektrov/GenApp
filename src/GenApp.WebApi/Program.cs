using GenApp.WebApi.Extensions;
using GenApp.WebApi.Middlewares;
using GenApp.DomainServices;
using GenApp.Templates.Parser;
using GenApp.Parsers.Csharp;
using GenApp.Parsers.Sql;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogLogging(builder.Configuration);
builder.Services.AddWebApi();
builder.Services.AddTemplateParser();
builder.Services.AddDomainServices();
builder.Services.AddCSharpParsers();
builder.Services.AddSqlParsers();

var app = builder.Build();

app.UseErrorHandling();

app.UseRequestLogger();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
