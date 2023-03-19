using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFullStackCrud.Server.Controllers
{
    /*      
     *  This class is used to :   
     *   | 
     *   . - > Get the data from the database 
     */
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {

        private readonly DataContext _context;
        
        // Constructor, where we inject the DataContext.
        public HealthRecordController(DataContext context)
        {
            _context = context;
        }

        //// Mock data
        //public static List<Allergies> allergies = new List<Allergies> { 
        
        //    new Allergies { AllergyId = 1, AllergyName= "Alergie la praf"},
        //    new Allergies { AllergyId = 2, AllergyName= "Alergie la lactoza"},
        //    new Allergies {AllergyId = 3, AllergyName = "test123"}
        //};

        //// Mock data
        //public static List<HealthRecord> healthrecords = new List<HealthRecord> {

        //    new HealthRecord { 
        //        PatientId = 1, 
        //        PatientName= "Ion", 
        //        MedicalHistory = "Healthy",
        //        Medications = "Pastile test",
        //        Allergies = allergies[0],
        //        AllergyId =1

        //    },

        //    new HealthRecord {
        //        PatientId = 2,
        //        PatientName= "John",
        //        MedicalHistory = "Healthy",
        //        Medications = "Pastile lactoza",
        //        Allergies = allergies[1],
        //        AllergyId = 2
        //    },

        //      new HealthRecord {
        //        PatientId = 1,
        //        PatientName= "Mark",
        //        MedicalHistory = "Very Healthy",
        //        Medications = "Pastile1212 lactoza",
        //        Allergies = allergies[2],
        //        AllergyId = 3
        //    },
        //};
        

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


        [HttpGet("allergies")]
        public async Task<ActionResult<List<Allergies>>> GetAllergies()
        {
            // Variable that is used to put all the allergies into a list.
            var allergies = await _context.Allergies.ToListAsync();

            // return status code 200
            return Ok(allergies);
        }

        [HttpGet("allergies/{id}")]
        public async Task<ActionResult<List<Allergies>>> GetAllergyById(int id)
        {
            // Variable that is used to get all the id's of the allergies.
            var allergies = await _context.Allergies
                .FirstOrDefaultAsync(a => a.AllergyId == id);

            // return status code 200
            return Ok(allergies);
        }



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
         * Implementation for the Create, Update & Delete on the Server.
         */

        // Create a record 
        [HttpPost]
        public async Task<ActionResult<List<HealthRecord>>> CreateHealthRecord(HealthRecordModel record)
        {
            //record.Allergies = null;
            if (record == null)
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

            //
            var result = await _context.HealthRecords.Include(sh => sh.Allergies).ToListAsync();
            //
            // return Ok(await GetDbRecords());
            return Ok(result);
            
        }

        // We use the route with the id, because we need to update the actual healthrecord
        // that we want to delete
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

        /* Return all our records to see the change in the database 
         *
         * Helpful when implementing the client, we will navigate back to the list of all our records.
         */
        private async Task<List<HealthRecord>> GetDbRecords()
        {
            return await _context.HealthRecords.Include(sh => sh.Allergies).ToListAsync();
        }
    }
}
