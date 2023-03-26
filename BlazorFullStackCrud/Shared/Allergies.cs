using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFullStackCrud.Shared
{
    public class Allergies
    {
        [Key]
        public int AllergyId { get; set; }

        public string AllergyName { get; set; } = string.Empty;

        public ICollection<HealthRecord>? HealthRecords { get; set; } = null!;
    }
}
