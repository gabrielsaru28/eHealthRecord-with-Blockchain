using Microsoft.AspNetCore.Mvc;
using BlazorFullStackCrud.Server.BlockchainServices;
using Nethereum.Hex.HexTypes;

namespace BlazorFullStackCrud.Server.Controllers
{

    /*
     * Attribute to the class definition to indicate that this is a controller that will handle HTTP requests:
     */
    [ApiController]
    [Route("api/[controller]")]
    public class BlockchainController : ControllerBase
    {
       
        // Add a constructor to the class that accepts an instance of the IBlockchainServices interface
        private readonly IBlockchainServices _blockchainServices;

        public BlockchainController(IBlockchainServices blockchainServices)
        {
            _blockchainServices = blockchainServices;
        }


        /*
         * The method is decorated with the [HttpPost] attribute to indicate that it will handle HTTP POST requests, 
         * and the {id} parameter in the attribute specifies that the method will accept a parameter called "id" in the URL
         *  
         */


        /*
         3. The SignHealthRecord method in the BlockchainController calls the SignHealthRecord method in the BlockchainServices class 
          to send a transaction to the signHealthRecord method on the smart contract with the ID of the selected health record.
         */
        [HttpPost("{id}")]
        public async Task<IActionResult> SignHealthRecord(int id)
        {
            try
            {

                /*
                 * Call the SignHealthRecord method of the BlockchainServices class, to send a transaction of the signHealthRecord method on the Smart Contract
                 * with the ID of the selected health record.
                 *
                 * The method returns the hash of the transaction.
                
                 * The transaction hash is sent back to the client as the response to the HTTP POST request.
                */
                //var uintId = new HexBigInteger(id);
                //var transactionHash = await _blockchainServices.SignHealthRecord(id);

                var transactionHash = await _blockchainServices.SignHealthRecord(id00);
                return Ok(transactionHash);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
         *
         * Handle the HTTP PUT requests to add a signature to a health record.
         *
         *  In this method, you will call the AddSignatureToHealthRecord method of the BlockchainServices class
         */

        /*
         *   5. The client receives the transaction hash and calls the AddSignatureToHealthRecord method on the server-side BlockchainController with the ID of the selected health record and the transaction hash.
         *
         *    6. The AddSignatureToHealthRecord method in the BlockchainController calls the AddSignatureToHealthRecord method in the BlockchainServices class to update the health record in the database with the transaction hash.
         */
        [HttpPost("blockchain/{id}")]
        public async Task<IActionResult> AddSignatureToHealthRecord(int id, [FromBody] string transactionHash)
        {
            try
            {
                await _blockchainServices.AddSignatureToHealthRecord(id, transactionHash);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
