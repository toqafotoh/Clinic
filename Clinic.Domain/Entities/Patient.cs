using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age => DateTime.Now.Year - BirthDate.Year;
        public ICollection<Appointment> Appointments { get; set; }
    }
}
