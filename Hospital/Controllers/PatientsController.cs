using AutoMapper;
using Hospital.DTO.Doctor;
using Hospital.DTO.Patient;
using Hospital.ORM;
using Hospital.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : Controller
    {
        private readonly PatientService _patientService;
        private readonly IMapper _mapper;

        public PatientsController(PatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientKeysDto>> GetPatient(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
                return NotFound();

            var patientDto = _mapper.Map<PatientKeysDto>(patient);

            return Ok(patientDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientValuesDto>>> GetPatients([FromQuery] int startIndex = 0,
                                                                             [FromQuery] int count = 10,
                                                                             [FromQuery] string sortBy = "id",
                                                                             [FromQuery] bool ascending = true)
        {
            var patients = await _patientService.GetPatientsAsync(startIndex, count, sortBy, ascending);

            var patientsDto = patients.Select(patient => _mapper.Map<PatientValuesDto>(patient));

            return Ok(patientsDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddPatient(AddPatientRequest addPatientRequest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var patient = _mapper.Map<Patient>(addPatientRequest);

                var addedPatient = await _patientService.AddPatientAsync(patient);

                var addedpatientDto = _mapper.Map<DoctorValuesDto>(addedPatient);

                return CreatedAtAction("GetPatient", new { id = addedpatientDto.Id }, addedpatientDto);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                    return BadRequest("One of foreign keys not found");

                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePatient(PatientKeysDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var patient = _mapper.Map<Patient>(patientDto);

                await _patientService.UpdatePatientAsync(patient);

                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException keyNotFoundException)
                    return NotFound();
                else
                    return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                    return NotFound();
                else
                    return StatusCode(500);
            }
        }
    }
}
