using Lambda.Pregiato.Interface;
using System.Text;

namespace Lambda.Pregiato.Services
{
    public class ContractServices : IContractService
    {

        public async Task<string> ConvertBytesToString(byte[] bytes)
        {
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Convertando contrato{bytes} para string.");
            return await Task.FromResult(Encoding.UTF8.GetString(bytes));
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Contrato:{bytes} convertido com sucesso.");
        }

        public async Task<byte[]> ExtractBytesFromString(string content)
        {
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} Extraindo bytes {content}.");
            int startIndex = content.IndexOf('[') + 1;
            int endIndex = content.LastIndexOf(']');

            string byteString = content.Substring(startIndex, endIndex - startIndex);

            byte[] bytes = byteString.Split(',')
                                     .Select(b => byte.Parse(b.Trim()))
                                     .ToArray();
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Extração finalizada com êxito.");
            return await Task.FromResult(bytes);
        }
    }
}