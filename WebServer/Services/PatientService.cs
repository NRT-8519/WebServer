using Microsoft.EntityFrameworkCore;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Models.UserData.Relations;
using WebServer.Services.Contexts;

namespace WebServer.Services
{

    public class PatientService : IDbService<Patient, PatientBasicDTO, PatientDetailsDTO>
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
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Include(p => p.AssignedDoctor.PersonalData)
                .Where(x => x.Roles.Any(r => r.Role.Equals("PATIENT"))).ToListAsync();
        }

        public async Task<Patient> FindById(int id)
        {
            return await context.Patients
                .Include(u => u.PersonalData)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Include(p => p.AssignedDoctor.PersonalData)
                .Where(x => x.Id == id && x.Roles.Any(r => r.Role.Equals("PATIENT"))).SingleOrDefaultAsync();
        }
        public async Task<Patient> FindByUUID(Guid UUID)
        {
            return await context.Patients
                .Include(u => u.PersonalData)
                .Include(u => u.Roles)
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Include(p => p.AssignedDoctor.PersonalData)
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
        public async Task<int> Update(PatientDetailsDTO entity)
        {
            try
            {
                Patient p = await FindByUUID(entity.UUID);
                p.UUID = entity.UUID;
                p.DoctorUUID = entity.AssignedDoctor.UUID;
                p.PersonalData.FirstName = entity.FirstName;
                p.PersonalData.MiddleName = entity.MiddleName;
                p.PersonalData.LastName = entity.LastName;
                p.PersonalData.Title = entity.Title;
                p.PersonalData.DateOfBirth = entity.DateOfBirth;
                p.PersonalData.SSN = entity.SSN;
                p.PersonalData.Gender = entity.Gender;
                p.PersonalData.Email = entity.Email;
                p.PersonalData.PhoneNumber = entity.PhoneNumber;

                p.IsDisabled = entity.IsDisabled;
                p.IsExpired = entity.IsExpired;
                p.PasswordExpiryDate = entity.PasswordExpiryDate;

                context.Update(p);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public Task<int> Delete(PatientBasicDTO entity)
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
