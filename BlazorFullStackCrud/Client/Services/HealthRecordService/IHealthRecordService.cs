using BlazorFullStackCrud.Shared;

namespace BlazorFullStackCrud.Client.Services.HealthRecordService
{
    public interface IHealthRecordService
    {
        List<HealthRecord> Records { get; set; }
        List<Allergies> Allergies { get; set; }
        Allergies AllergyName { get; set; }

        Task GetAllergies();
        Task GetHealthRecords();

        Task<HealthRecord> GetSingleHealthRecord(int id);
        Task<Allergies> GetAllergyById(int id);


        /*
         * CRUD Operations Methods
         */
        Task CreateRecord(HealthRecord record);
        Task UpdateRecord(HealthRecord record);
        Task DeleteRecord(int id);


        // Blockchain related methods
        Task SignHealthRecord(int patientId);

       // Task AddHealthRecordToBlockchain(HealthRecord record, string privateKey);
    }
}
