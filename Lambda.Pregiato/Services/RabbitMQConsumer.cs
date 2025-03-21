using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Lambda.Pregiato.Services.ModelService;
using System.Text.Json;
using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Data;

public class RabbitMQConsumer : IRabbitMQConsumer
{
    private readonly string _queueName;
    private readonly string _rabbitMqUri;
    private readonly IContractRepository _contractRepository;
    private readonly IContractService _contractService;
    private readonly IModelRepository _modelRepository;
    private readonly IAutentiqueService _autentiqueService;
    private readonly LambdaContextDB _lambdaContextDB;
    private readonly ConnectionFactory _factory;

    public RabbitMQConsumer(
        IContractRepository contractRepository,
        IContractService contractService,
        IModelRepository modelRepository,
        IAutentiqueService autentiqueService,
        LambdaContextDB lambdaContext)
    {
        _rabbitMqUri = Environment.GetEnvironmentVariable("RABBITMQ_URI")?? "amqps://guest:guest@localhost:5672";
        _queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE") ?? "sqs-inboud-sendfile";

        _contractRepository = contractRepository;
        _contractService = contractService;
        _lambdaContextDB = lambdaContext;
        _modelRepository = modelRepository;
        _autentiqueService = autentiqueService; 

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
                    await ProcessMessage(contractMessage);
                    receivedMessage = message;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erro ao processar mensagem: {ex.Message}");
            }
        };

        await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("Aguardando mensagens... Pressione Ctrl+C para sair.");

        while (string.IsNullOrEmpty(receivedMessage))
        {
            await Task.Delay(Timeout.Infinite);
        }
    }

    public async Task<string> ProcessMessage(ContractMessage contractMessage)
     {
        if (string.IsNullOrEmpty(contractMessage.CpfModel) && string.IsNullOrEmpty( contractMessage.ContractIds.ToString()))
        {
            throw new Exception("Parametros para envio de contratos vazio.");
        }

        var model = await _modelRepository.GetModelByCriteriaAsync(contractMessage.CpfModel);

        if (model == null) { throw new Exception("Modelos não encontrado."); }
        
        foreach (var id in contractMessage.ContractIds)
        {
            if (Guid.TryParse(id, out Guid contractId))
            {
                var contract = await _contractRepository.GetContractById(contractId);
                if (contract != null)
                {
                    try
                    {
                        string contentString = await _contractService.ConvertBytesToString(contract.Content);
                        byte[] pdfBytes = await _contractService.ExtractBytesFromString(contentString);
                        string pdfBase64 = Convert.ToBase64String(pdfBytes);
                        string nameFile = contract.ContractFilePath;
                        var result = await _autentiqueService.CreateDocumentAsync(nameFile, pdfBase64, model);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Erro ao criar documento: {ex.Message}\n{ex.StackTrace}");
                    }
                }
                else
                {
                    Console.WriteLine($" Erro: Contrato ID {contractId} não encontrado.");
                }
            }
        }
        return "Processamento concluído.";
    }
}


