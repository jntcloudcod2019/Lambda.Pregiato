using Lambda.Pregiato.Interface;
using System.Text;

namespace Lambda.Pregiato.Services
{
    public class ContractServices : IContractService
    {
        public async Task<string> ConvertBytesToString(byte[] bytes)
        {
            try
            {
                if (bytes != null && bytes.Length > 0)
                {
                    Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Conversão do contrato {bytes} para string.");
                    return Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Erro: Bytes do contrato estão vazios ou nulos.");
                    return null; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Erro ao converter bytes para string: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]> ExtractBytesFromString(string content)
        {
            try
            {
                int startIndex = content.IndexOf('[') + 1;
                int endIndex = content.LastIndexOf(']');

                if (startIndex < 0 || endIndex < 0)
                {
                    Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Erro: Formato do conteúdo inválido. Não foi possível encontrar os delimitadores '[' e ']'.");
                    return null; 
                }

                string byteString = content.Substring(startIndex, endIndex - startIndex);

                byte[] bytes = byteString.Split(',')
                                         .Select(b => byte.Parse(b.Trim()))
                                         .ToArray();

                Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Extraídos {bytes.Length} bytes do conteúdo.");
                return bytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Erro ao extrair bytes do conteúdo: {ex.Message}");
                return null; 
            }
        }
    }
}