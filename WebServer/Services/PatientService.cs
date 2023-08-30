using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
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

        public async Task<IEnumerable<PatientDetailsDTO>> FindAll()
        {
            var result = await context.Patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Where(x => x.Role.Equals("PATIENT")).ToListAsync();

            List<PatientDetailsDTO> DTOs = new();
            foreach (var patient in result)
            {
                if (patient.AssignedDoctor == null)
                {
                    DTOs.Add(new PatientDetailsDTO
                    {
                        UUID = patient.UUID,
                        FirstName = patient.FirstName,
                        MiddleName = patient.MiddleName,
                        LastName = patient.LastName,
                        Username = patient.Username,
                        Title = patient.Title,
                        DateOfBirth = patient.DateOfBirth,
                        Gender = patient.Gender,
                        SSN = patient.SSN,
                        Email = patient.Email,
                        PhoneNumber = patient.PhoneNumber,
                        PasswordExpiryDate = patient.PasswordExpiryDate,
                        IsDisabled = patient.IsDisabled,
                        IsExpired = patient.IsExpired
                    });
                }
                else
                {
                    DTOs.Add(new PatientDetailsDTO
                    {
                        UUID = patient.UUID,
                        FirstName = patient.FirstName,
                        MiddleName = patient.MiddleName,
                        LastName = patient.LastName,
                        Username = patient.Username,
                        Title = patient.Title,
                        DateOfBirth = patient.DateOfBirth,
                        Gender = patient.Gender,
                        SSN = patient.SSN,
                        Email = patient.Email,
                        PhoneNumber = patient.PhoneNumber,
                        PasswordExpiryDate = patient.PasswordExpiryDate,
                        IsDisabled = patient.IsDisabled,
                        IsExpired = patient.IsExpired,
                        AssignedDoctor = new UserBasicDTO
                        {
                            FirstName = patient.AssignedDoctor.FirstName,
                            MiddleName = patient.AssignedDoctor.MiddleName,
                            LastName = patient.AssignedDoctor.LastName,
                            Username = patient.AssignedDoctor.Username,
                            Email = patient.AssignedDoctor.Email,
                            UUID = patient.AssignedDoctor.UUID
                        }
                    });
                }
                
            }

            return DTOs;
        }

        public async Task<PaginatedResultDTO<PatientDetailsDTO>> FindAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
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

            var result = await PaginatedList<Patient>.CreateAsync(patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<PatientDetailsDTO> DTOs = new()
            {
                PageNumber = result.PageIndex,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalItems = result.TotalItems,
                HasNext = result.HasNextPage,
                HasPrevious = result.HasPreviousPage
            };

            foreach (var patient in result)
            {
                PatientDetailsDTO p = new()
                {
                    UUID = patient.UUID,
                    FirstName = patient.FirstName,
                    MiddleName = patient.MiddleName,
                    LastName = patient.LastName,
                    Username = patient.Username,
                    Title = patient.Title,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    SSN = patient.SSN,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    PasswordExpiryDate = patient.PasswordExpiryDate,
                    IsDisabled = patient.IsDisabled,
                    IsExpired = patient.IsExpired,
                    AssignedDoctor = new UserBasicDTO
                    {
                        FirstName = patient.AssignedDoctor.FirstName,
                        MiddleName = patient.AssignedDoctor.MiddleName,
                        LastName = patient.AssignedDoctor.LastName,
                        Username = patient.AssignedDoctor.Username,
                        Email = patient.AssignedDoctor.Email,
                        UUID = patient.AssignedDoctor.UUID
                    }
                };

                DTOs.items.Add(p);
            }

            return DTOs;
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
        public async Task<Patient> FindEntityByUUID(Guid UUID)
        {
            Patient p = await context.Patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Where(x => x.UUID.Equals(UUID) && x.Role.Equals("PATIENT")).SingleOrDefaultAsync();


            return p;
        }

        public async Task<PatientDetailsDTO> FindByUUID(Guid UUID)
        {
            Patient result = await context.Patients
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(p => p.AssignedDoctor)
                .Where(x => x.UUID.Equals(UUID) && x.Role.Equals("PATIENT")).SingleOrDefaultAsync();

            PatientDetailsDTO patient = new()
            {
                UUID = result.UUID,
                FirstName = result.FirstName,
                MiddleName = result.MiddleName,
                LastName = result.LastName,
                Username = result.Username,
                Title = result.Title,
                DateOfBirth = result.DateOfBirth,
                Gender = result.Gender,
                SSN = result.SSN,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                PasswordExpiryDate = result.PasswordExpiryDate,
                IsDisabled = result.IsDisabled,
                IsExpired = result.IsExpired,
                AssignedDoctor = new UserBasicDTO
                {
                    FirstName = result.AssignedDoctor.FirstName,
                    MiddleName = result.AssignedDoctor.MiddleName,
                    LastName = result.AssignedDoctor.LastName,
                    Username = result.AssignedDoctor.Username,
                    Email = result.AssignedDoctor.Email,
                    UUID = result.AssignedDoctor.UUID
                }
            };
            return patient;
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
                Patient p = await FindEntityByUUID(entity.UUID);
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
