using BlazorFullStackCrud.Shared;

namespace BlazorFullStackCrud.Client.Services.HealthRecordService
{
    public interface IHealthRecordService
    {
        List<HealthRecord> Records { get; set; }
        List<Allergies> Allergies { get; set; }
        List<Allergies> Allergies2 { get; set; }


        Allergies AllergyName { get; set; }

        Task GetAllergies();
        Task GetHealthRecords();

        Task<HealthRecord> GetSingleHealthRecord(int id);
        Task<Allergies> GetAllergyById(int id);

        //string GetAllergyName(int id);

        /*
         * CRUD Operations Methods
         */
        Task CreateRecord(HealthRecord record);
        Task UpdateRecord(HealthRecord record);
        Task DeleteRecord(int id);

       // Task<string> SemneazaRecord(int patientId);


    }
}
