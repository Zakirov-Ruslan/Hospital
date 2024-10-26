using AutoMapper;
using Hospital.DTO.Doctor;
using System.Reflection;
using Hospital.DTO;
using Hospital.DTO.Doctor;
using Hospital.ORM;
using Hospital.DTO.Patient;

namespace Hospital
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Area, AreaDTO>().ReverseMap();
            CreateMap<Cabinet, CabinetDTO>().ReverseMap();
            CreateMap<Specialization, SpecializationDTO>().ReverseMap();

            CreateMap<AddDoctorRequest, Doctor>();
            CreateMap<DoctorValuesDto, Doctor>().ReverseMap();
            CreateMap<DoctorKeysDto, Doctor>().ReverseMap();

            CreateMap<AddPatientRequest, Patient>();
            CreateMap<PatientValuesDto, Patient>().ReverseMap();
            CreateMap<PatientKeysDto, Patient>().ReverseMap();
        }
    }
}
