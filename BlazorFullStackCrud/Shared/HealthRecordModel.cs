using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFullStackCrud.Shared
{
    public class HealthRecordModel
    {

        public string PatientName { get; set; } = string.Empty;

        public string MedicalHistory { get; set; } = string.Empty;

        public string Medications { get; set; } = string.Empty;

        public int AllergyId { get; set; }

    }
}
