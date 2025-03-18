using Lambda.Pregiato.DTO;
using Lambda.Pregiato.Models;
using System.Diagnostics.Contracts;

namespace Lambda.Pregiato.Interface
{
    public interface IAutentiqueService
    {
        Task<string> CreateDocumentAsync(string documentBase64, string documentName);
    }
}
