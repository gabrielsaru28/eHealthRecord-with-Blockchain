using Nethereum.Contracts;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3.Accounts;
using BlazorFullStackCrud.Client.Services.HealthRecordService;
using Nethereum.ABI.Model;

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
        private readonly string _ganacheUrl;
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
        public BlockchainServices(IConfiguration config, DataContext dbContext)
        {
            _contractAddress = config.GetValue<string>("Blockchain:ContractAddress");
            _accountAddress = config.GetValue<string>("Blockchain:AccountAddress");
            _accountPrivateKey = config.GetValue<string>("Blockchain:MetaMaskPrivateKey");
            _web3 = new Web3(config.GetValue<string>("Blockchain:NodeWebsocketUrl"));
            _dbContext = dbContext;
            var contractAbiJson = File.ReadAllText("contractabi.json");
            var contractAbi = JObject.Parse(contractAbiJson)["Abi"].ToString();
            _contract = _web3.Eth.GetContract(contractAbi, _contractAddress);
            var ganacheUrl = config["Blockchain:GanacheUrl"];
            var account = new Account("0x6362fbe9c5dd15dafe9dd344f2757df9ba5045f343053bdfe824da193cce5a76");
            _web3 = new Web3(account, ganacheUrl);
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

            var web3 = new Web3("http://localhost:8545");
            // Sender account
            var privateKey = "0xc5a23241f9529cf308c0666dc3efd5222be63673235c649f73466b9e939f8f2f";
            var senderAddress = "0x329e7cF730D04646E962B94921Ab7b5AC14b39E7"; // Replace with the actual sender address
            var account = new Account(privateKey);
            var recipientAddress = "0x329e7cF730D04646E962B94921Ab7b5AC14b39E7"; // Replace with the desired recipient address

            // Send transaction
            var transactionHash = await web3.TransactionManager.SendTransactionAsync(
                new Nethereum.RPC.Eth.DTOs.TransactionInput
                {
                    From = senderAddress,
                    To = recipientAddress,
                    Value = new Nethereum.Hex.HexTypes.HexBigInteger(1), // Amount (in wei)
                    Gas = new Nethereum.Hex.HexTypes.HexBigInteger(21000) // Gas limit
                });
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