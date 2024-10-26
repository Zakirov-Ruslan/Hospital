using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Doctor
{
    public class DoctorKeysDto
    {
        [Required]
        public int Id { get; set; }
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
