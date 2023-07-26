using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.UserData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public sealed class UserService : IDbService<User>
    {
        private readonly UserContext context;

        public UserService(UserContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> FindAll()
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).ToListAsync();
        }

        public async Task<IEnumerable<User>> FindAllDisabled()
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).Where(u => u.IsDisabled).ToListAsync();
        }
        public async Task<IEnumerable<User>> FindAllExpired()
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).Where(u => u.IsExpired).ToListAsync();
        }
        public async Task<User> FindById(int id)
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User> FindByUUID(Guid UUID)
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.UUID.Equals(UUID));
        }
        public async Task<User> FindByUsername(string username)
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.Username.Equals(username));
        }
        public async Task<User> FindByEmail(string email)
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.Emails.Any(e => e.Email.Equals(email)));
        }
        public async Task<User> FindByPhoneNumber(string phoneNo)
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.PhoneNumbers.Any(e => e.PhoneNumber.Equals(phoneNo)));
        }
        public async Task<IEnumerable<User>> FindByRole(string role)
        {
            return await context.Users.Include(u => u.PersonalData).Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).Where(x => x.Roles.Any(e => e.Role.Equals(role))).ToListAsync();
        }
        public async Task<IEnumerable<Doctor>> FindAllDoctors()
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
        public async Task<Doctor> FindDoctorById(int id)
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
        public async Task<Doctor> FindDoctorByUUID(Guid UUID)
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

        public async Task<IEnumerable<Patient>> FindAllPatients()
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
        public async Task<Patient> FindPatientById(int id)
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
        public async Task<Patient> FindPatientByUUID(Guid UUID)
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
        public async Task<int> Insert(User entity)
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
        public async Task<int> Update(User entity)
        {
            try
            {
                context.Update(entity);
                return await context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> InsertDoctor(Doctor entity)
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
        public async Task<int> UpdateDoctor(Doctor entity)
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

        public async Task<int> InsertPatient(Patient entity)
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
        public async Task<int> UpdatePatient(Patient entity)
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

        public async Task<int> Delete(User entity)
        {
            try
            {
                context.Remove(entity);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteById(int id)
        {
            try
            {
                User user = await FindById(id);
                context.Users.Remove(user);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteByUUID(Guid UUID)
        {
            try
            {
                User user = await FindByUUID(UUID);
                context.Users.Remove(user);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }
    }
}
