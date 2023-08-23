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

        public async Task<PaginatedList<Doctor>> FindAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var doctors = from d in context.Doctors where d.UUID != Guid.Empty select d;
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

            return await PaginatedList<Doctor>.CreateAsync(doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffs)
                .Include(d => d.WorkShifts), pageNumber ?? 1, pageSize);
            //return await context.Doctors
            //    .Include(d => d.Notes)
            //    .Include(d => d.Prescriptions)
            //    .Include(d => d.Schedules)
            //    .Include(d => d.TimeOffs)
            //    .Include(d => d.WorkShifts)
            //    .Where(x => x.Role.Equals("DOCTOR")).ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> FindAll()
        {
            return await context.Doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffs)
                .Include(d => d.WorkShifts)
                .Where(x => x.Role.Equals("DOCTOR")).ToListAsync();
        }
        public async Task<Doctor> FindById(int id)
        {
            return await context.Doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffs)
                .Include(d => d.WorkShifts)
                .Where(x => x.Id == id && x.Role.Equals("DOCTOR")).SingleOrDefaultAsync();
        }
        public async Task<Doctor> FindByUUID(Guid UUID)
        {
            return await context.Doctors
                .Include(d => d.Notes)
                .Include(d => d.Prescriptions)
                .Include(d => d.Schedules)
                .Include(d => d.TimeOffs)
                .Include(d => d.WorkShifts)
                .Where(x => x.UUID.Equals(UUID) && x.Role.Equals("DOCTOR")).SingleOrDefaultAsync();
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
                Doctor d = await FindByUUID(entity.UUID);
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
