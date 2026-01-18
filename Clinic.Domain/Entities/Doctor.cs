using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public ICollection<DoctorSchedule> Schedules { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
