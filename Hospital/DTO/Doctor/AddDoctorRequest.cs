using System.ComponentModel.DataAnnotations;
using Hospital.ORM;

namespace Hospital.DTO.Doctor
{
    public class AddDoctorRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public int CabinetId { get; set; }

        [Required]
        public int SpecializationId { get; set; }

        [Required]
        public int AreaId { get; set; }
    }
}
