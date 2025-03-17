using Lambda.Pregiato.Data;
using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Configuration.AddConfiguration(config);

var connectionString = config.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LambdaContextDB>(options =>
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
           .LogTo(Console.WriteLine, LogLevel.Information));


builder.Services.AddDbContext<LambdaContextDB>();
builder.Services.AddScoped<RabbitMQConsumer>();
builder.Services.AddScoped<IContractService, ContractServices>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IModelRepository, ModelRepository>();    

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();
    var consumer = scope.ServiceProvider.GetRequiredService<RabbitMQConsumer>();
    consumer.StartConsuming();
});

app.Run();
