using Lambda.Pregiato.Interface;
using Lambda.Pregiato.Models;
using Microsoft.EntityFrameworkCore;

namespace Lambda.Pregiato.Data
{
    public class ModelRepository : IModelRepository  
    {
        private readonly LambdaContextDB _lambdaContext;

        public ModelRepository(LambdaContextDB lambdaContext)
        {
            _lambdaContext = lambdaContext;
        }

        public async Task<Model?> GetModelByCriteriaAsync(string query)
        {

          Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Buscando dados do modelo | Parameters: {query}.");
          
            return await _lambdaContext.Model.FirstOrDefaultAsync(m =>
          m.CPF == query ||
          m.RG == query ||
          m.Name.Contains(query) ||
          m.IdModel.ToString() == query);
        }
    }
}
