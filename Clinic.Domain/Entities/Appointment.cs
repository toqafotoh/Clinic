using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int VisitDuration { get; set; } = 30;
        public TimeSpan EndTime => StartTime.Add(TimeSpan.FromMinutes(VisitDuration));
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
