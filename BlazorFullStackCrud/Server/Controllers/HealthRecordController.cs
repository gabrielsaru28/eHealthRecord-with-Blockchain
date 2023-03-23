using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFullStackCrud.Server.Controllers
{
    /*      
     *  This class is used to :   
     *   | 
     *   . - > Get the data from the database 
     */

    /*  
     *  ===============================
     *  == HealthRecordController.cs ==
     *  ===============================
     *  
     *  Component of the application that receives the incoming HTTP requests and processes them.
     *  
     *  It controls the data flow in the application and interacts with the Service layer to retrieve or update the data.
     *  
     *  The Controller, takes in the data received from the UI, transforms it, if needed, passes it to the Service layer for processing, and return the response to the UI.
     *  
     *  Used in the server part of the application to implement API endpoints that are called from the client-side Blazor code.
     * 
     *  Handles HTTP requests and responses, perform data validation, and interact with the database using Entity Framework Core.
     */

    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        /*
         * private instance of the `DataContext` class, which is being injected via constructor injection in `public HealthRecordController(DataContext context)` constructor.
         */
        private readonly DataContext _context;
        
        // Constructor, where we inject the DataContext.
        public HealthRecordController(DataContext context)
        {
            _context = context;
        }


        /*
         *  This is a GET API endpoint that returns a list of HealthRecord objects from a database. 
         *  
         *  The method asynchronously retrieves the HealthRecords from the database and includes related Allergies data, and stores it into a list variable called healthrecords. 
         *  
         *  The retrieved data is then returned with a 200 HTTP status code using the Ok method, which is an ActionResult type.
         */
        [HttpGet]
        public async Task<ActionResult<List<HealthRecord>>> GetHealthRecords()
        {
            // Retrieve data from database
            var healthrecords = await _context
                .HealthRecords
                .Include( a =>  a.Allergies)
                .ToListAsync();
            
            // return status code 200
            return Ok(healthrecords);
        }

        /*
         *  This is a GET API endpoint that returns a list of Allergies objects from a database. 
         *  
         *  The method asynchronously retrieves Allergies data from the database and stores it into a list variable called "allergies". 
         *  
         *  The retrieved data is then returned with a 200 HTTP status code using the Ok method, which is an ActionResult type. 
         */
        [HttpGet("allergies")]
        public async Task<ActionResult<List<Allergies>>> GetAllergies()
        {
            // Retrieve data from database
            // Put the extracted data from the database into a variable, that is used to put all the allergies into a list.
            var allergies = await _context.Allergies.ToListAsync();

            // return status code 200
            return Ok(allergies);
        }


        /*
         *  This is a GET API endpoint that returns a single Allergies object from a database based on the given ID. 
         *  
         *  The method takes an integer ID as input and asynchronously retrieves the first Allergies object that matches the ID from the database. 
         *  
         *  The retrieved data is then returned with a 200 HTTP status code using the Ok method, which is an ActionResult type.
         */
        [HttpGet("allergies/{id}")]
        public async Task<ActionResult<List<Allergies>>> GetAllergyById(int id)
        {
            // Variable that is used to get all the id's of the allergies.
            var allergies = await _context.Allergies
                .FirstOrDefaultAsync(a => a.AllergyId == id);

            // return status code 200
            return Ok(allergies);
        }


        /*
         *  This is a GET API endpoint that returns a single HealthRecord object from a database based on the given PatientId. 
         *  
         *  The method takes an integer ID as input and asynchronously retrieves the first HealthRecord object that matches the PatientId from the database. 
         *  
         *  If no such HealthRecord exists, a 404 HTTP status code with an error message "No healthrecord here" is returned using the NotFound method. 
         *  
         *  If the HealthRecord is found, it is returned with a 200 HTTP status code using the Ok method, which is an ActionResult type.
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthRecord>> GetSingeHealthRecord(int id)
        {
            var record = await _context.HealthRecords
                .FirstOrDefaultAsync( h => h.PatientId == id);
           
            if (record == null)
            {
                return NotFound("No healthrecord here");
            }

            // return status code 200
            return Ok(record);
        }


        /*
         *  This method retrieves a list of `HealthRecord` objects from the database with their associated `Allergies` data included.
         *  
         *  It is defined in a HealthController class in the Controllers folder of a Blazor WebAssembly App, and returns a task that resolves to the list of `HealthRecord` objects.
         */
        private async Task<List<HealthRecord>> GetDbRecords()
        {
            return await _context.HealthRecords.Include(sh => sh.Allergies).ToListAsync();
        }


        /*
         * Implementation for the Create, Update & Delete on the Server.
         */

        /*  
         *  This is a POST API endpoint that creates a new HealthRecord object in the database based on the input provided in the request body. 
         *  
         *  The method takes a HealthRecordModel object as input, which is used to create a new HealthRecord object called "hrecord". 
         *  
         *  If the input "record" is null, the method returns a BadRequest HTTP status code. 
         *  
         *  Otherwise, the new object is added to the HealthRecords DbSet, saved to the database with SaveChangesAsync, 
         *  and a list of all HealthRecord objects is retrieved from the database, including related Allergies data, and returned with a 200 HTTP status code using the Ok method, which is an ActionResult type. 
         *  
         *  This controller method is used in a Blazor WebAssembly app to handle HTTP POST requests from the client-side.  
         *   
         */

        [HttpPost]
        public async Task<ActionResult<List<HealthRecord>>> CreateHealthRecord(HealthRecord record)
        {
            
            /*  Checking if the record is NULL   */
            if (record == null)
                // If it is NULL, then it returns a BadRequest
                return BadRequest();
           

            HealthRecord hrecord = new HealthRecord
            {
                PatientName = record.PatientName,
                MedicalHistory = record.MedicalHistory,
                Medications = record.Medications,
                AllergyId = record.AllergyId
            };


            _context.HealthRecords.Add(hrecord);
            await _context.SaveChangesAsync();

          
            var result = await _context.HealthRecords.Include(sh => sh.Allergies).ToListAsync();  
            // return Ok(await GetDbRecords());
            return Ok(result);
            
        }

        /*
         *   This is a PUT API endpoint that updates an existing HealthRecord object in the database based on the input provided in the request body and the provided ID in the request URL. 
         *   
         *   The method retrieves the original HealthRecord object from the database based on the ID provided, includes related Allergies data, and updates its properties with the values provided in the input HealthRecord object. 
         *   
         *   If no HealthRecord object is found with the provided ID, a NotFound HTTP status code is returned. 
         *   
         *   Otherwise, the updated object is saved to the database with SaveChangesAsync and a list of all HealthRecord objects is retrieved from the database, 
         *   including related Allergies data, and returned with a 200 HTTP status code using the Ok method, which is an ActionResult type.
         *   
         *   This controller method is used in a Blazor WebAssembly app to handle HTTP PUT requests from the client-side.
         */
        [HttpPut("{id}")]
        public async Task<ActionResult<List<HealthRecord>>> UpdateHealthRecord(HealthRecord record, int id)
        {
            var dbRecord =   await _context.HealthRecords
                .Include(sh => sh.Allergies)
                .FirstOrDefaultAsync(sh => sh.PatientId == id);

            if (dbRecord == null)
                return NotFound("Sorry, but no record for you");

            // Override the properties manually
            dbRecord.PatientName = record.PatientName;
            dbRecord.MedicalHistory = record.MedicalHistory;
            dbRecord.Medications = record.Medications;
            dbRecord.AllergyId = record.AllergyId;

            await _context.SaveChangesAsync();

            return Ok(await GetDbRecords());
        }


        /*
         *  This is a DELETE API endpoint that deletes an existing HealthRecord object from the database based on the provided ID in the request URL. 
         *  
         *  The method retrieves the HealthRecord object from the database based on the ID provided, includes related Allergies data, and removes it from the HealthRecords DbSet. 
         *  
         *  If no HealthRecord object is found with the provided ID, a NotFound HTTP status code is returned. 
         *  
         *  Otherwise, the object is removed from the database with SaveChangesAsync and a list of all HealthRecord objects is retrieved from the database, 
         *  including related Allergies data, and returned with a 200 HTTP status code using the Ok method, which is an ActionResult type. 
         *  
         *  This controller method is used in a Blazor WebAssembly app to handle HTTP DELETE requests from the client-side.
         *
         */
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<HealthRecord>>> DeleteHealthRecord( int id)
        {
            var dbRecord = await _context.HealthRecords
                .Include(sh => sh.Allergies)
                .FirstOrDefaultAsync(sh => sh.PatientId == id);

            if (dbRecord == null)
                return NotFound("Sorry, but no record for you");

          
            _context.HealthRecords.Remove(dbRecord);
            await _context.SaveChangesAsync();

            return Ok(await GetDbRecords());
        }
    }
}
