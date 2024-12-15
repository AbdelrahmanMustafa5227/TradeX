using TradeX.Api.Mapping;
using TradeX.Api.Middleware;
using TradeX.Application;
using TradeX.Infrastructure;
using TradeX.Api.Extensions;
using TradeX.Application.Abstractions;
using TradeX.Api.Misc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IUserContext,UserContext>();
builder.Services.AddHttpContextAccessor();

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