using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json.Linq;

namespace BlazorFullStackCrud.Server.BlockchainServices
{
    public class BlockchainServices : IBlockchainServices
    {
        private readonly string _contractAddress;
        private readonly string _abi;
        private readonly Web3 _web3;
        private readonly string _accountAddress;
        private readonly string _accountPrivateKey;
        private readonly Contract _contract;
        private readonly DataContext _dbContext;

        /*
         * The constructor for the BlockchainService class takes the following parameters:
         *
         * web3: An instance of the Web3 class, which provides access to the Ethereum blockchain.
         * 
         * contractAddress: The address of the deployed smart contract on the Ethereum blockchain.
         * 
         * abi: The Application Binary Interface (ABI) of the smart contract, which specifies its methods and parameters.
         * 
         * accountAddress: The Ethereum address of the account that will be used to sign the transaction.
         * 
         * dbContext: An instance of the ApplicationDbContext class, which provides access to the database.
         */
        public BlockchainServices(IConfiguration config)
        {
            _contractAddress = config.GetValue<string>("Blockchain:ContractAddress");
            //_abi = config.GetValue<string>("Blockchain:Abi");
            _accountAddress = config.GetValue<string>("Blockchain:AccountAddress");
            _accountPrivateKey = config.GetValue<string>("Blockchain:MetaMaskPrivateKey");
            _web3 = new Web3(config.GetValue<string>("Blockchain:NodeWebsocketUrl"));
            //_contract = _web3.Eth.GetContract(_abi, _contractAddress);
            //_dbContext = new Data.DataContext();
        
            var contractAbiJson = File.ReadAllText("contractabi.json");
            var contractAbi = JObject.Parse(contractAbiJson)["Abi"].ToString();
        
            _contract = _web3.Eth.GetContract(contractAbi, _contractAddress);
        }


        /*
         * The SignHealthRecord method takes an id parameter, and sends a transaction to the signHealthRecord method on the smart contract with that ID. 
         * 
         * The method returns the hash of the transaction.
         */
        /*
         4. The SignHealthRecord method in the BlockchainServices class returns the transaction hash, which is sent back to the client as the response to the HTTP POST request.
         */
        public async Task<string> SignHealthRecord(int id)
        {
            var function = _contract.GetFunction("signHealthRecord");
            //var transactionInput = function.CreateTransactionInput(_accountAddress, new { id });
            var transactionInput = function.CreateTransactionInput(_accountAddress, new { id = new Nethereum.Hex.HexTypes.HexBigInteger((ulong)id) });

            var transactionHash = await _web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);
            return transactionHash;
        }

        /*
         * The AddSignatureToHealthRecord method takes a transactionHash and updates the health record with the corresponding ID in the database with the signature.
         */
        public async Task AddSignatureToHealthRecord(int healthRecordId, string transactionHash)
        {
            var healthRecord = await _dbContext.HealthRecords.FirstOrDefaultAsync(h => h.PatientId == healthRecordId);
            
            // If the health record exists, update the signature with the transaction hash and save the changes to the database.
            if (healthRecord != null)
            {
                healthRecord.Signature = transactionHash;
                _dbContext.HealthRecords.Update(healthRecord);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
