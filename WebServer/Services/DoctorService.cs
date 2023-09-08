using Microsoft.EntityFrameworkCore;
using WebServer.Features;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class DoctorService : IDbService<Doctor, UserBasicDTO, DoctorDetailsDTO>
    {
        private readonly UserContext context;
        public DoctorService(UserContext context)
        {
            this.context = context;
        }

        public async Task<PaginatedResultDTO<DoctorDetailsDTO>> FindAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var doctors = context.Doctors.Include(d => d.Notes).Include(d => d.Prescriptions).Include(d => d.Schedules).Where(d => !d.UUID.Equals(Guid.Empty) && d.Role.Equals("DOCTOR")).AsQueryable();
            var patients = await context.Patients.Include(d => d.Notes).Include(d => d.Prescriptions).Include(d => d.Schedules).Include(p => p.AssignedDoctor).Where(x => x.Role.Equals("PATIENT")).ToListAsync();
            if (true)
            {
                if (searchQuery != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchQuery = currentFilter;
                }

                if (!String.IsNullOrEmpty(searchQuery))
                {
                    doctors = doctors.Where(d =>
                        d.FirstName.Contains(searchQuery) ||
                        d.MiddleName.Contains(searchQuery) ||
                        d.LastName.Contains(searchQuery) ||
                        d.Title.Contains(searchQuery) ||
                        d.Email.Contains(searchQuery) ||
                        d.PhoneNumber.Contains(searchQuery) ||
                        d.Username.Contains(searchQuery) ||
                        d.AreaOfExpertise.Contains(searchQuery) ||
                        d.RoomNumber.ToString().Contains(searchQuery) ||
                        d.UUID.ToString().Contains(searchQuery)
                    );
                }
            }

            var result = await PaginatedList<Doctor>.CreateAsync(doctors.AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<DoctorDetailsDTO> DTOs = new();
            DTOs.PageNumber = result.PageIndex;
            DTOs.PageSize = result.PageSize;
            DTOs.TotalPages = result.TotalPages;
            DTOs.TotalItems = result.TotalItems;
            DTOs.HasNext = result.HasNextPage;
            DTOs.HasPrevious = result.HasPreviousPage;

            foreach (var doctor in result)
            {
                List<UserBasicDTO> patientsList = new();
                foreach (var p in patients)
                {
                    if (p.AssignedDoctor != null)
                    {
                        if (p.AssignedDoctor.UUID.Equals(doctor.UUID))
                        {
                            patientsList.Add(new UserBasicDTO
                            {
                                UUID = p.UUID,
                                FirstName = p.FirstName,
                                MiddleName = p.MiddleName,
                                LastName = p.LastName,
                                Username = p.Username,
                                Email = p.Email
                            });
                        }
                    }
                }
                DoctorDetailsDTO d = new()
                {
                    UUID = doctor.UUID,
                    FirstName = doctor.FirstName,
                    MiddleName = doctor.MiddleName,
                    LastName = doctor.LastName,
                    Username = doctor.Username,
                    Title = doctor.Title,
                    DateOfBirth = doctor.DateOfBirth,
                    Gender = doctor.Gender,
                    SSN = doctor.SSN,
                    Email = doctor.Email,
                    PhoneNumber = doctor.PhoneNumber,
                    PasswordExpiryDate = doctor.PasswordExpiryDate,
                    IsDisabled = doctor.IsDisabled,
                    IsExpired = doctor.IsExpired,
                    AreaOfExpertise = doctor.AreaOfExpertise,
                    RoomNumber = doctor.RoomNumber,
                    Patients = patientsList
                };

                DTOs.items.Add(d);
            }

            return DTOs;
        }

        public async Task<IEnumerable<DoctorDetailsDTO>> FindAll()
        {
            var result = await context.Doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Where(d => !d.UUID.Equals(Guid.Empty) && d.Role.Equals("DOCTOR")).ToListAsync();
            var patients = await context.Patients.Include(d => d.Notes).Include(d => d.Prescriptions).Include(d => d.Schedules).Include(p => p.AssignedDoctor).Where(p => p.Role.Equals("PATIENT")).ToListAsync();


            List<DoctorDetailsDTO> DTOs = new();
            foreach (var doctor in result)
            {
                List<UserBasicDTO> patientsList = new();
                foreach (var p in patients)
                {
                    if (p.AssignedDoctor != null)
                    {
                        if (p.AssignedDoctor.UUID.Equals(doctor.UUID))
                        {
                            patientsList.Add(new UserBasicDTO
                            {
                                UUID = p.UUID,
                                FirstName = p.FirstName,
                                MiddleName = p.MiddleName,
                                LastName = p.LastName,
                                Username = p.Username,
                                Email = p.Email
                            });
                        }
                    }
                }

                DTOs.Add(new()
                {
                    UUID = doctor.UUID,
                    FirstName = doctor.FirstName,
                    MiddleName = doctor.MiddleName,
                    LastName = doctor.LastName,
                    Username = doctor.Username,
                    Title = doctor.Title,
                    DateOfBirth = doctor.DateOfBirth,
                    Gender = doctor.Gender,
                    SSN = doctor.SSN,
                    Email = doctor.Email,
                    PhoneNumber = doctor.PhoneNumber,
                    PasswordExpiryDate = doctor.PasswordExpiryDate,
                    IsDisabled = doctor.IsDisabled,
                    IsExpired = doctor.IsExpired,
                    AreaOfExpertise = doctor.AreaOfExpertise,
                    RoomNumber = doctor.RoomNumber,
                    Patients = patientsList
                });
            }

            return DTOs;
        }
        public async Task<Doctor> FindById(int id)
        {
            return await context.Doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Where(x => x.Id == id && x.Role.Equals("DOCTOR")).SingleOrDefaultAsync();
        }
        public async Task<Doctor> FindEntityByUUID(Guid UUID)
        {
            return await context.Doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Where(x => x.UUID.Equals(UUID) && x.Role.Equals("DOCTOR")).SingleOrDefaultAsync();
        }

        public async Task<DoctorDetailsDTO> FindByUUID(Guid UUID)
        {
            var result = await context.Doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Where(x => x.UUID.Equals(UUID) && x.Role.Equals("DOCTOR")).AsNoTracking().SingleOrDefaultAsync();
            
            if (result != null)
            {
                var patients = await context.Patients.Include(d => d.Notes).Include(d => d.Prescriptions).Include(d => d.Schedules).Include(p => p.AssignedDoctor).Where(x => x.Role.Equals("PATIENT")).AsNoTracking().ToListAsync();

                List<UserBasicDTO> patientsList = new();
                foreach (var p in patients)
                {
                    if (p.AssignedDoctor.UUID.Equals(result.UUID))
                    {
                        patientsList.Add(new UserBasicDTO
                        {
                            UUID = p.UUID,
                            FirstName = p.FirstName,
                            MiddleName = p.MiddleName,
                            LastName = p.LastName,
                            Username = p.Username,
                            Email = p.Email
                        });
                    }
                }

                DoctorDetailsDTO doctor = new()
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
                    AreaOfExpertise = result.AreaOfExpertise,
                    RoomNumber = result.RoomNumber,
                    Patients = patientsList
                };

                return doctor;
            }
            else
            {
                return null;
            }
        }

        public async Task<int> Insert(DoctorDetailsDTO entity)
        {
            try
            {
                Guid UUID = Guid.NewGuid();

                Doctor d = new() { 
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
                    AreaOfExpertise = entity.AreaOfExpertise,
                    RoomNumber = entity.RoomNumber,
                    Password = Password.Generate(15, 5),
                    IsDisabled = false,
                    IsExpired = false,
                    PasswordExpiryDate = DateTime.Now.AddMonths(6),
                    Role = "Doctor"
                };

                context.Add(d);

                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }
        public async Task<int> Update(DoctorDetailsDTO entity)
        {
            try
            {
                Doctor d = await FindEntityByUUID(entity.UUID);
                d.UUID = entity.UUID;
                d.FirstName = entity.FirstName;
                d.MiddleName = entity.MiddleName;
                d.LastName = entity.LastName;
                d.Title = entity.Title;
                d.Email = entity.Email;
                d.PhoneNumber = entity.PhoneNumber;
                d.DateOfBirth = entity.DateOfBirth;
                d.SSN = entity.SSN;
                d.Gender = entity.Gender;
                d.Username = entity.Username;
                if (entity.Password != null && !entity.Password.Equals(""))
                {
                    d.Password = entity.Password;
                }
                d.AreaOfExpertise = entity.AreaOfExpertise;
                d.RoomNumber = entity.RoomNumber;
                d.IsDisabled = entity.IsDisabled;
                d.IsExpired = entity.IsExpired;
                d.PasswordExpiryDate = entity.PasswordExpiryDate;

                context.Update(d);

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
