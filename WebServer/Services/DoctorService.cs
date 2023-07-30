using Microsoft.EntityFrameworkCore;
using WebServer.Models.ClinicData.Entities;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class DoctorService : IDbService<Doctor>
    {
        private readonly UserContext context;

        public DoctorService(UserContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Doctor>> FindAll()
        {
            return await context.Doctors
                .Include(u => u.PersonalData)
                .Include(u => u.Emails)
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffs)
                .Include(d => d.WorkShifts)
                .Where(x => x.Roles.Any(r => r.Role.Equals("DOCTOR"))).ToListAsync();
        }
        public async Task<Doctor> FindById(int id)
        {
            return await context.Doctors
                .Include(u => u.PersonalData)
                .Include(u => u.Emails)
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffs)
                .Include(d => d.WorkShifts)
                .Where(x => x.Id == id && x.Roles.Any(r => r.Role.Equals("DOCTOR"))).SingleOrDefaultAsync();
        }
        public async Task<Doctor> FindByUUID(Guid UUID)
        {
            return await context.Doctors
                .Include(u => u.PersonalData)
                .Include(u => u.Emails)
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffs)
                .Include(d => d.WorkShifts)
                .Where(x => x.UUID.Equals(UUID) && x.Roles.Any(r => r.Role.Equals("DOCTOR"))).SingleOrDefaultAsync();
        }

        public async Task<int> Insert(Doctor entity)
        {
            try
            {
                context.Add(entity);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }
        public async Task<int> Update(Doctor entity)
        {
            try
            {
                context.Update(entity);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public Task<int> Delete(Doctor entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
