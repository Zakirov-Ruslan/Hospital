using Microsoft.EntityFrameworkCore;
using Hospital.ORM;
using System.Numerics;

namespace Hospital.Services
{
    public class PatientService
    {
        private readonly HospitalContext _context;

        public PatientService(HospitalContext context)
        {
            _context = context;
        }

        public async Task<Patient> AddPatientAsync(Patient patient)
        {
            var areaExists = _context.Areas.Any(a => a.Id == patient.AreaId);

            if (!areaExists)
                throw new KeyNotFoundException();

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            var existingPatient = _context.Patients.Find(patient.Id);
            if (existingPatient == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Entry(existingPatient).CurrentValues.SetValues(patient);

            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public async Task<List<Patient>> GetPatientsAsync(int startIndex, int count, string sortBy, bool ascending = true)
        {
            var query = _context.Patients.AsQueryable();

            // Sorting by property
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var propertyInfo = typeof(Patient).GetProperty(sortBy, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    query = ascending ? query.OrderBy(e => EF.Property<object>(e, propertyInfo.Name))
                                      : query.OrderByDescending(e => EF.Property<object>(e, propertyInfo.Name));
                }
            }

            // Executing query with pagination
            query = query.Skip(startIndex).Take(count).Include(p => p.Area);

            return await query.ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            var patient = await _context.Patients.Include(d => d.Area).
                                                  FirstOrDefaultAsync(d => d.Id == id);

            return patient;
        }
    }
}
