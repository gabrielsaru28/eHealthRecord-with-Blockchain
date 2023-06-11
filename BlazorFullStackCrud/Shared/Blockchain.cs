using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFullStackCrud.Shared
{
    public class Blockchain
    {
        public string AlchemyApiKey { get; set; }
        public string MetaMaskPrivateKey { get; set; }
        public string NodeWebsocketUrl { get; set; }
        public string GanacheUrl { get; set; }

        public string ContractAddress { get; set; }
        public string Abi { get; set; }
        public string AccountAddress { get; set; }

    }
}
