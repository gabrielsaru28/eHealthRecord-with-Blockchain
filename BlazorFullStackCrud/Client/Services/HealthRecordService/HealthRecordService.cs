using Microsoft.AspNetCore.Components;
using NBitcoin.Secp256k1;
using System.Net.Http.Json;
using System.Text.Json;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using System.Text;
using Nethereum.Contracts;
using System.Numerics;
using Nethereum.Web3.Accounts;
using System.Net.Http;

namespace BlazorFullStackCrud.Client.Services.HealthRecordService
{
    /*
     *  ===========================
     *  === HealthRecordService ===
     *  ===========================
     *  
     *  1. Responsible for processing the data that is received from the Controller.
     *  
     *  2. It is essentially the "business logic" of the application, where the data is processed, validated, and transformed as required.
     *  
     *  3. The Service layer is designed to be independent of the UI layer, allowing it to be reused across different platforms and application.
     *
     *  4. It receives input from the Controller, performs operations on the data, and returns the result to the Controller.
     */
    public class HealthRecordService : IHealthRecordService
    {
        private readonly HttpClient _http;

        private readonly NavigationManager _navigationManager;

        public HealthRecordService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<HealthRecord> Records { get; set; } = new List<HealthRecord>();

        public List<Allergies> Allergies { get; set; } = new List<Allergies>();


        public HttpClient Http { get; }

        public Allergies AllergyName { get; set; }
     

        /*
         *   Method triggered when pressing 'Create New Record' in the UI.
         */
        public async Task CreateRecord(HealthRecord record)
        {
            // result is an HttpResponseMessage ( we do not get the list of the healthrecords directly)
            var result = await _http.PostAsJsonAsync("api/healthrecord", record); 
            //var content = await result.Content.ReadAsStringAsync();          
            //HealthRecord data = JsonSerializer.Deserialize<HealthRecord>(content);
            await SetRecords(result);
        }

        /*
         *   Method triggered when pressing 'Update Record' after pressing 'Edit' in the UI.
         */
        public async Task UpdateRecord(HealthRecord record)
        {
            // result is an HttpResponseMessage ( we do not get the list of the healthrecords directly)
            var result = await _http.PutAsJsonAsync($"api/healthrecord/{record.PatientId}", record);
          //  var response = await result.Content.ReadFromJsonAsync<List<HealthRecord>>();
            await SetRecords(result);
        }

        /*
         *   Method triggered when pressing 'Delete Record' after pressing 'Edit' in the UI.
         */
        public async Task DeleteRecord(int id)
        {
            // result is an HttpResponseMessage ( we do not get the list of the healthrecords directly)
            var result = await _http.DeleteAsync($"api/healthrecord/{id}");
            // var response = await result.Content.ReadFromJsonAsync<List<HealthRecord>>();
            await SetRecords(result);
        }

        /*
         * This method navigates to the 'healthrecords' page after receiving a response from an HTTP request.
         */
        private async Task SetRecords(HttpResponseMessage result)
        {
            //var response = await result.Content.ReadFromJsonAsync<List<HealthRecord>>();
            //var response = await result.Content.ReadFromJsonAsync<List<HealthRecord>>();
           // Records = response;
            _navigationManager.NavigateTo("healthrecords");   
        }


        public async Task GetAllergies()
        {
            var result = await _http.GetFromJsonAsync<List<Allergies>>("api/healthrecord/allergies");
            if (result != null)
                Allergies = result;
        }

        
        public async Task<Allergies> GetAllergyById(int id)
        {
            var result = await _http.GetFromJsonAsync<Allergies>($"api/healthrecord/allergies/{id}");
            if (result != null)
            {
                AllergyName = result;
                
            }
            throw new Exception("Allergies not found!");
        }


        /*   
         *  Retrieves a list of health records from the API by making an HTTP GET request to the "api/healthrecord" endpoint. 
         *    
         *  It uses the _http instance of the HttpClient class to make this request. 
         *    
         *  The response from the API is deserialized into a List<HealthRecord> object using the GetFromJsonAsync() method
         */



        /// <summary>
        /// Retrieves a list of health records from the API and updates the Records property of the HealthRecordService class with the retrieved data.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>

        public async Task GetHealthRecords()
        {
            var result = await _http.GetFromJsonAsync<List<HealthRecord>>("api/healthrecord");
            if (result != null)
                Records = result;
        }

        // Retrieves a single health record from the API by making an HTTP GET request to the "api/healthrecord/{id}" endpoint.
        public async Task<HealthRecord> GetSingleHealthRecord(int id)
        {
            var result = await _http.GetFromJsonAsync<HealthRecord>($"api/healthrecord/{id}");
            if (result != null)
                return result;
            throw new Exception("HealthRecord not found!");
        }

        // Signs a health record by making an HTTP POST request to the "api/blockchain/{id}" endpoint.
        /*
         * We're using the _http in this method to call the API endpoint, defined in the BlockchainController, and retrieve the transaction hash.
         *
         * We're then using the _http again to call the 'AddSignatureToHealthRecord' API endpoint, defined in the HealthRecordController, to add the transaction hash to the health record in the database.
         */
        public async Task SignHealthRecord(int id)
        {
            // Call the SignHealthRecord API endpoint in the BlockchainController
            var response = await _http.PostAsync($"api/blockchain/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                // Get the transaction hash from the response body
                var transactionHash = await response.Content.ReadAsStringAsync();

                // Add the transaction hash to the health record in the database
                await _http.PostAsJsonAsync($"api/healthrecords/{id}/addsignature", transactionHash);

                // Refresh the health record list
                await GetHealthRecords();
            }
            else
            {
                // Handle the error
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

    }
}
