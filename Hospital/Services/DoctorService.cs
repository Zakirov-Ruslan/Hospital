using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Hospital.DTO;
using Hospital.ORM;

namespace Hospital.Services
{
    public class DoctorService
    {
        private readonly HospitalContext _context;

        public DoctorService(HospitalContext context)
        {
            _context = context;
        }

        public async Task<Doctor> AddDoctorAsync(Doctor doctor)
        {
            var cabinetExists = _context.Cabinets.Any(c => c.Id == doctor.CabinetId);
            var specializationExists = _context.Specializations.Any(s => s.Id == doctor.SpecializationId);
            var areaExists = _context.Areas.Any(a => a.Id == doctor.AreaId);

            if (!cabinetExists || !specializationExists || !areaExists)
                throw new KeyNotFoundException();

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return doctor;
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            var existingDoctor = _context.Doctors.Find(doctor.Id);
            if (existingDoctor == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Entry(existingDoctor).CurrentValues.SetValues(doctor);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public async Task<List<Doctor>> GetDoctorsAsync(int startIndex, int count, string sortBy, bool ascending)
        {
            var query = _context.Doctors.AsQueryable();

            // Sorting by property
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var propertyInfo = typeof(Doctor).GetProperty(sortBy, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    query = ascending ? query.OrderBy(e => EF.Property<object>(e, propertyInfo.Name))
                                      : query.OrderByDescending(e => EF.Property<object>(e, propertyInfo.Name));
                }
            }

            // Executing query with pagination
            query = query.Skip(startIndex).
                          Take(count).
                          Include(d => d.Cabinet).
                          Include(d => d.Specialization).
                          Include(d => d.Area);

            return await query.ToListAsync();
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors.Include(d => d.Specialization).
                                                Include(d => d.Area).
                                                Include(d => d.Cabinet).
                                                FirstOrDefaultAsync(d => d.Id == id);

            return doctor;
        }
    }
}
