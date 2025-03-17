using Lambda.Pregiato.Data;
using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//  Carregando as configurações
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Configuration.AddConfiguration(config);

//  Configurando o banco de dados
var connectionString = config.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LambdaContextDB>(options =>
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
           .LogTo(Console.WriteLine, LogLevel.Information));

//  Injetando dependências
builder.Services.AddScoped<RabbitMQConsumer>();
builder.Services.AddScoped<IContractService, ContractServices>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IModelRepository, ModelRepository>();

// Adicionando suporte a controllers
builder.Services.AddControllers();

//  Configurando Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Lambda Pregiato API",
        Version = "v1",
        Description = "Documentação da API para integração com RabbitMQ",
        Contact = new OpenApiContact
        {
            Name = "Seu Nome",
            Email = "seuemail@exemplo.com",
            Url = new Uri("https://github.com/seu-usuario")
        }
    });
});

var app = builder.Build();

// Configurando Middleware do Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lambda Pregiato API v1");
        c.RoutePrefix = string.Empty; // Swagger disponível na raiz do app
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var scope = app.Services.CreateScope();
var consumer = scope.ServiceProvider.GetRequiredService<RabbitMQConsumer>();

Task.Run(() => consumer.StartConsuming());
app.Urls.Add("http://localhost:7070");
app.Run();
