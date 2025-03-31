using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Models;
using Microsoft.EntityFrameworkCore;

namespace Lambda.Pregiato.Data
{
    public class ContractRepository : IContractRepository
    {
        private readonly LambdaContextDB _lambdaContext;

        public ContractRepository(LambdaContextDB lambdaContext) 
        {
             _lambdaContext = lambdaContext;
        }

        public async Task<Contract?> GetContractById(Guid id)
        {

           return await _lambdaContext.Contracts
               .AsTracking()
               .Where(c => c.ContractId == id) 
          .Select(c => new Contract  
          {
              ContractId = c.ContractId,   
              ContractFilePath =c.ContractFilePath,
              Content = c.Content,
              CodProducers = c.CodProducers
          })
          .FirstOrDefaultAsync();
        }
    }
}
