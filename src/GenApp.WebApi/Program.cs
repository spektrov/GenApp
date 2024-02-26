using GenApp.WebApi.Extensions;
using GenApp.WebApi.Middlewares;
using GenApp.DomainServices;
using GenApp.Templates.Parser;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogLogging(builder.Configuration);
builder.Services.AddWebApi();
builder.Services.AddTemplateParser();
builder.Services.AddDomainServices();

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
