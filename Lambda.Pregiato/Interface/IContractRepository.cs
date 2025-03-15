using Lambda.Pregiato.Models;

namespace Lambda.Pregiato.Interface
{
    public interface IContractRepository
    {
        Task<Contract> GetContractById(Guid contractId);
      
    }
}
