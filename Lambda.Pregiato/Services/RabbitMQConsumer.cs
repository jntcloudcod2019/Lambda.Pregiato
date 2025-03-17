using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Lambda.Pregiato.Services.ModelService;
using System.Text.Json;
using RabbitMQ.Client;
using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Models;
using Lambda.Pregiato.Services;
using System.Diagnostics.Contracts;
using Lambda.Pregiato.Data;

public class RabbitMQConsumer: IRabbitMQConsumer
{
    private readonly string _queueName;
    private readonly string _rabbitMqUri;
    private readonly IContractRepository _contractRepository;
    private readonly IContractService _contractService;
    private readonly LambdaContextDB _lambdaContextDB;
    private readonly IModelRepository _modelRepository;
    private readonly ConnectionFactory _factory;

    public RabbitMQConsumer( IContractRepository contractRepository, IContractService contractService, IModelRepository modelRepository ,LambdaContextDB lambdaContext)
    {
        _rabbitMqUri = Environment.GetEnvironmentVariable("RABBITMQ_URI")
            ?? "amqps://guest:guest@localhost:5672"; 

        _queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE")
            ?? "sqs-inboud-sendfile";  
        
        _contractRepository = contractRepository;
        _contractService = contractService;
        _lambdaContextDB = lambdaContext;
        _modelRepository = modelRepository;

        _factory = new ConnectionFactory()
        {
            HostName = "mouse.rmq5.cloudamqp.com",
            VirtualHost = "ewxcrhtv",
            UserName = "ewxcrhtv",
            Password = "DNcdH0NEeP4Fsgo2_w-vd47CqjelFk_S",
            Port = 5672,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };
    }

    public async Task StartConsuming()
    {
        using var connection = await _factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var consumer = new AsyncEventingBasicConsumer(channel);
        string receivedMessage = string.Empty; 

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($" Mensagem Recebida: {message}");
            try
            {
                var contractMessage = JsonSerializer.Deserialize<ContractMessage>(message);

                if (contractMessage != null)
                {
                    ProcessMessage(contractMessage);
                    receivedMessage = message;
                }
                else
                {
                    Console.WriteLine(" Mensagem inválida");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erro ao processar mensagem: {ex.Message}");
            }

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("Aguardando mensagens... Pressione Ctrl+C para sair.");

        while (string.IsNullOrEmpty(receivedMessage))
        {
            await Task.Delay(500); 
        }

    }

    public async Task ProcessMessage(ContractMessage contractMessage)
    {


        foreach (var id in contractMessage.ContractIds)
        {
            if (Guid.TryParse(id, out Guid contractId))
            {
                Console.WriteLine($" Buscando contrato com ID: {contractId}");

                var contract = await _contractRepository.GetContractById(contractId);

                if (contract != null)
                {
                    string contentString = await _contractService.ConvertBytesToString(contract.Content);
                    byte[] pdfBytes;
                    pdfBytes = await _contractService.ExtractBytesFromString(contentString);
                    
                    var contractFle = File.WriteAllBytesAsync(contract.ContractFilePath ,pdfBytes);                 

                    var model = await _modelRepository.GetModelByCriteriaAsync(contractMessage.CpfModel);

                    if (model == null) { Console.WriteLine("Modelo não encontrado."); }
                }
                else
                {
                    Console.WriteLine($" Contrato não encontrado para ID: {contractId}");
                }
            }        
        }
    }
}


