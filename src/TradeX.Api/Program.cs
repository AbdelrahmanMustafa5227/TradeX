using TradeX.Api.Middleware;
using TradeX.Application;
using TradeX.Infrastructure;
using TradeX.Api.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Host.ConfigureLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandelingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program;