namespace BlazorFullStackCrud.Server.Data
{
    /* 
     * Used to acces the data from the database 
     * We'll inject the data context to do that.
     */

    /*
     * Contains/Extends the DbContext class that represents the database context for the application. 
     *
     * This class is responsible for managing the database connection, querying the database, and tracking changes to the data
     *
     */


    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<HealthRecord>()
                .HasOne<Allergies>(hr => hr.Allergies)
                .WithMany(a => a.HealthRecords)
                .HasForeignKey(hr => hr.AllergyId); 


            // Mock data for Allergies.
            modelBuilder.Entity<Allergies>().HasData(

                 new Allergies { AllergyId = 1, AllergyName = "Alergie la praf" },
                 new Allergies { AllergyId = 2, AllergyName = "Alergie la lactoza" },
                 new Allergies { AllergyId = 3, AllergyName = "Alergie la capsuni"}
                );

            // Mock data for HealthRecords
            modelBuilder.Entity<HealthRecord>().HasData(

                new HealthRecord
                {
                    PatientId = 1,
                    PatientName = "Ion",
                    MedicalHistory = "Healthy",
                    Medications = "Ibuprofen",
                    AllergyId = 1

                },

                new HealthRecord
                {
                    PatientId = 2,
                    PatientName = "John",
                    MedicalHistory = "Healthy",
                    Medications = "Nurofen",
                    AllergyId = 2
                },

                new HealthRecord
                {
                    PatientId = 3,
                    PatientName = "Mark",
                    MedicalHistory = "Very Healthy",
                    Medications = "Aspirina",
                    AllergyId = 3
                }
                 );

            base.OnModelCreating(modelBuilder);
        }

        /*
         * Very important ---The DbSets (Database sets)
         *
         * Whenever you want to see an entity represented as a table in your Db
         * You have to add the database set
        */
        
        // Tabels
        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<Allergies> Allergies  { get; set; }

    }
}
