using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Hospital.ORM
{
    public class Area
    {
        public int Id { get; set; } 
        public int Number { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
    }
}