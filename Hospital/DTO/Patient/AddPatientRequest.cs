using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Patient
{
    public class AddPatientRequest
    {
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public int AreaId { get; set; }
    }
}