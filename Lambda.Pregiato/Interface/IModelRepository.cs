using Lambda.Pregiato.Models;

namespace Lambda.Pregiato.Interface
{
    public interface IModelRepository
    {
        Task<Model?> GetModelByCriteriaAsync(string query);
    }
}
