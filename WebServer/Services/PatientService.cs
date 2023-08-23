using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.Controllers;
using WebServer.Features;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Services.Contexts;

namespace WebServer.Services
{

    public class PatientService : IDbService<Patient, UserBasicDTO, PatientDetailsDTO>
    {
        private readonly UserContext context;

        public PatientService(UserContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Patient>> FindAll()
        {
            return await context.Patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Where(x => x.Role.Equals("PATIENT")).ToListAsync();
        }

        public async Task<PaginatedList<Patient>> FindAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var patients = from p in context.Patients select p;

            if (searchQuery != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchQuery = currentFilter;
            }

            if(!String.IsNullOrEmpty(searchQuery))
            {
                patients = patients.Where(p =>
                    p.FirstName.Contains(searchQuery) ||
                    p.MiddleName.Contains(searchQuery) ||
                    p.LastName.Contains(searchQuery) ||
                    p.Title.Contains(searchQuery) ||
                    p.Email.Contains(searchQuery) ||
                    p.PhoneNumber.Contains(searchQuery) ||
                    p.Username.Contains(searchQuery) ||
                    p.UUID.ToString().Contains(searchQuery)
                );
            }

            return await PaginatedList<Patient>.CreateAsync(patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<Patient> FindById(int id)
        {
            return await context.Patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Where(x => x.Id == id && x.Role.Equals("PATIENT")).SingleOrDefaultAsync();
        }
        public async Task<Patient> FindByUUID(Guid UUID)
        {
            Patient p = await context.Patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Where(x => x.UUID.Equals(UUID) && x.Role.Equals("PATIENT")).SingleOrDefaultAsync();


            return p;
        }

        public async Task<int> Insert(PatientDetailsDTO entity)
        {
            try
            {
                Guid UUID = Guid.NewGuid();

                Patient p = new()
                {
                    UUID = UUID,
                    FirstName = entity.FirstName,
                    MiddleName = entity.MiddleName,
                    LastName = entity.LastName,
                    Title = entity.Title,
                    DateOfBirth = entity.DateOfBirth,
                    SSN = entity.SSN,
                    Gender = entity.Gender,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    Username = entity.Username,
                    Password = Password.Generate(15, 5),
                    IsDisabled = false,
                    IsExpired = false,
                    AssignedDoctor = null,
                    PasswordExpiryDate = DateTime.Now.AddMonths(6),
                    Role = "PATIENT"
                };

                context.Add(p);

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
                p.FirstName = entity.FirstName;
                p.MiddleName = entity.MiddleName;
                p.LastName = entity.LastName;
                p.Username = entity.Username;
                if (entity.Password != null && !entity.Password.Equals(""))
                {
                    p.Password = entity.Password;
                }
                p.Title = entity.Title;
                p.DateOfBirth = entity.DateOfBirth;
                p.SSN = entity.SSN;
                p.Gender = entity.Gender;
                p.Email = entity.Email;
                p.PhoneNumber = entity.PhoneNumber;

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

        public Task<int> Delete(UserBasicDTO entity)
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
