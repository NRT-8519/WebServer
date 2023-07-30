using Microsoft.EntityFrameworkCore;
using WebServer.Models.ClinicData.Entities;
using WebServer.Services.Contexts;

namespace WebServer.Services
{

    public class PatientService : IDbService<Patient>
    {
        private readonly UserContext context;

        public PatientService(UserContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Patient>> FindAll()
        {
            return await context.Patients
                .Include(u => u.PersonalData)
                .Include(u => u.Emails)
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Where(x => x.Roles.Any(r => r.Role.Equals("PATIENT"))).ToListAsync();
        }

        public async Task<Patient> FindById(int id)
        {
            return await context.Patients
                .Include(u => u.PersonalData)
                .Include(u => u.Emails)
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Where(x => x.Id == id && x.Roles.Any(r => r.Role.Equals("PATIENT"))).SingleOrDefaultAsync();
        }
        public async Task<Patient> FindByUUID(Guid UUID)
        {
            return await context.Patients
                .Include(u => u.PersonalData)
                .Include(u => u.Emails)
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Where(x => x.UUID.Equals(UUID) && x.Roles.Any(r => r.Role.Equals("PATIENT"))).SingleOrDefaultAsync();
        }

        public async Task<int> Insert(Patient entity)
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
        public async Task<int> Update(Patient entity)
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

        public Task<int> Delete(Patient entity)
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
