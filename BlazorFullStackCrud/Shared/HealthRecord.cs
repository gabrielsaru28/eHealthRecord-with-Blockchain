using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFullStackCrud.Shared
{
    /*
     * This class is for the Health Record table
     * It has a collection of Allergies, because a health record can have many allergies, but an allergy can only have one Health Record.
     *
     * So the collection of allergies is in the Health Record class, and the FK is in the Allergies class.
     -> The foreign key is the allergy id
     --> this is a many - to - one relationship.
     
     -> The allergy id is the foreign key
     --> so this is a one - to - many relationship.
   
     */
    public class HealthRecord
    {
        [Key]
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string MedicalHistory { get; set; } = string.Empty;

        public string Medications { get; set; } = string.Empty;

        public int? AllergyId { get; set; }

        public Allergies? Allergies { get; set; } = null!;

        public string Signature { get; set; } = string.Empty;
    }
}
