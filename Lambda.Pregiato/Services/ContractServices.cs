using Lambda.Pregiato.Interface;
using System.Text;

namespace Lambda.Pregiato.Services
{
    public class ContractServices : IContractService
    {
        private readonly Logger _logger; 

        public ContractServices()
        {
            _logger = new Logger();
        }

        public async Task<string> ConvertBytesToString(byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                _logger.LogInfo($"Conversão do contrato {bytes} para string.");
                return Encoding.UTF8.GetString(bytes);
            }
            else
            {
                _logger.LogError("Erro: Bytes do contrato estão vazios ou nulos.");
                throw new ArgumentException("Bytes do contrato não podem ser nulos ou vazios.");
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
                    _logger.LogError("Erro: Formato do conteúdo inválido. Não foi possível encontrar os delimitadores '[' e ']'.");
                    throw new FormatException("Formato do conteúdo inválido.");
                }

                string byteString = content.Substring(startIndex, endIndex - startIndex);

                byte[] bytes = byteString.Split(',')
                                         .Select(b => byte.Parse(b.Trim()))
                                         .ToArray();

                _logger.LogInfo($"Extraídos {bytes.Length} bytes do conteúdo.");
                return bytes;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao extrair bytes do conteúdo.", ex);
                throw;
            }
        }
    }
}