using AutoMapper;
using Hospital.DTO.Doctor;
using Microsoft.AspNetCore.Mvc;
using Hospital.DTO.Doctor;
using Hospital.ORM;
using Hospital.Services;

namespace Hospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly IMapper _mapper;
        public DoctorsController(DoctorService doctorService, IMapper mapper)
        {
            _doctorService = doctorService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorKeysDto>> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
                return NotFound();

            var doctorDto = _mapper.Map<DoctorKeysDto>(doctor);

            return Ok(doctorDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorValuesDto>>> GetDoctors([FromQuery] int startIndex = 0,
                                                                                 [FromQuery] int count = 10,
                                                                                 [FromQuery] string sortBy = "id",
                                                                                 [FromQuery] bool ascending = true)
        {
            var doctors = await _doctorService.GetDoctorsAsync(startIndex, count, sortBy, ascending);

            var doctorsDto = doctors.Select(doctor => _mapper.Map<DoctorValuesDto>(doctor));

            return Ok(doctorsDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddDoctor([FromBody] AddDoctorRequest addDoctorRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var doctor = _mapper.Map<Doctor>(addDoctorRequest);

                var addedDoctor = await _doctorService.AddDoctorAsync(doctor);

                var addedDoctorDto = _mapper.Map<DoctorKeysDto>(addedDoctor);

                return CreatedAtAction("GetDoctor", new { id = addedDoctorDto.Id }, addedDoctorDto);
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                    return BadRequest("One of foreign keys not found");

                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateDoctor([FromBody] DoctorKeysDto updateDoctorRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var doctor = _mapper.Map<Doctor>(updateDoctorRequest);

                await _doctorService.UpdateDoctorAsync(doctor);

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
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            try
            {
                await _doctorService.DeleteDoctorAsync(id);

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
