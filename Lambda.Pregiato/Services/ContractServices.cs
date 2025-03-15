using Lambda.Pregiato.Interface;
using System.Text;

namespace Lambda.Pregiato.Services
{
    public class ContractServices :IContractService 
    {

        public async Task<string> ConvertBytesToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public async Task<byte[]> ExtractBytesFromString(string content)
        {
            int startIndex = content.IndexOf('[') + 1;
            int endIndex = content.LastIndexOf(']');

            string byteString = content.Substring(startIndex, endIndex - startIndex);

            byte[] bytes = byteString.Split(',')
                                     .Select(b => byte.Parse(b.Trim()))
                                     .ToArray();

            return bytes;
        }
    }
}
