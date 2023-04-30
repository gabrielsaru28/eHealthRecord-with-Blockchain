using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFullStackCrud.Shared
{
    /*
     * This class is for the Allergies table
     * It has a collection of Health Records, because a health record can have many allergies, but an allergy can only have one Health Record.
     *
     * So the collection of health records is in the allergies class, and the FK is in the Health Record class.
     -> The foreign key is the allergy id
     --> this is a many - to - one relationship.
    
    -> The allergy id is the foreign key
    --> so this is a one - to - many relationship.
  
    */
    public class Allergies
    {
        [Key]
        public int AllergyId { get; set; }

        public string AllergyName { get; set; } = string.Empty;

        public ICollection<HealthRecord>? HealthRecords { get; set; } = null!;
    }
}
