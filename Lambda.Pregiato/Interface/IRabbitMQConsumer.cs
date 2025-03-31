using Lambda.Pregiato.Services.ModelService;

namespace Lambda.Pregiato.Interface
{
    public interface IRabbitMQConsumer
    {
        public Task StartConsuming();
        Task<string> ProcessMessage(ContractMessage contractMessage);
    }
}
