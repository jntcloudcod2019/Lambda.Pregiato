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

        public async Task<Contract> GetContractById(Guid id)
        {

          return await _lambdaContext.Contracts.FirstOrDefaultAsync( c => c.ContractId == id );   
                               
        }
    }
}
