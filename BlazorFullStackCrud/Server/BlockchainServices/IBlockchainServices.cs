namespace BlazorFullStackCrud.Server.BlockchainServices
{
    public interface IBlockchainServices
    {
        Task<string> SignHealthRecord(int id);
        Task AddSignatureToHealthRecord(int id, string transactionHash);
    }
}
