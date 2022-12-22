using Microsoft.AspNetCore.OData;
using static CreditCardSimulator.Startup.Odata.EdmModelBuilder;
using static CreditCardSimulator.Startup.Util;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData((opt => opt.AddRouteComponents("odata", GetEdmModel()).Filter().Select().Expand().EnableQueryFeatures()));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureServices(ConfigureServices);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//var dbConnectionString = "";
//builder.Services.AddDbContext<CreditCardSimulator.Models.MagicBankContext>(
//                   options => options.UseNpgsql(dbConnectionString));
app.UseHttpsRedirection();

app.UseODataRouteDebug("odata");
app.UseAuthorization();

app.MapControllers();

app.Run();
