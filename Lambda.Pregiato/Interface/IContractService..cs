namespace Lambda.Pregiato.Interface
{
    public interface IContractService
    {
        Task<string> ConvertBytesToString(byte[] bytes);

        Task<byte[]> ExtractBytesFromString(string content);
    }
}
