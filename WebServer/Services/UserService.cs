using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using WebServer.Features;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Models.UserData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public sealed class UserService : IDbService<User, UserBasicDTO, UserDetailsDTO>
    {
        private readonly UserContext context;

        public UserService(UserContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> FindAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<PaginatedList<User>> FindAllAdministratorsPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var administrators = from a in context.Users where a.Role.Equals("ADMINISTRATOR") select a;
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
                administrators = administrators.Where(a =>
                    a.FirstName.Contains(searchQuery) ||
                    a.MiddleName.Contains(searchQuery) ||
                    a.LastName.Contains(searchQuery) ||
                    a.Title.Contains(searchQuery) ||
                    a.Email.Contains(searchQuery) ||
                    a.PhoneNumber.Contains(searchQuery) ||
                    a.Username.Contains(searchQuery) ||
                    a.UUID.ToString().Contains(searchQuery)
                );
            }

            return await PaginatedList<User>.CreateAsync(administrators.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<IEnumerable<User>> FindAllDisabled()
        {
            return await context.Users.Where(u => u.IsDisabled).ToListAsync();
        }
        public async Task<IEnumerable<User>> FindAllExpired()
        {
            return await context.Users.Where(u => u.IsExpired).ToListAsync();
        }
        public async Task<User> FindById(int id)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User> FindByUUID(Guid UUID)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.UUID.Equals(UUID));
        }
        public async Task<User> FindAdministratorByUUID(Guid UUID)
        {
            return await context.Users.Where(x => x.Role.Equals("ADMINISTRATOR")).SingleOrDefaultAsync(x => x.UUID.Equals(UUID));
        }
        public async Task<User> FindByUsername(string username)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.Username.Equals(username));
        }
        public async Task<User> FindByEmail(string email)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));
        }
        public async Task<User> FindByPhoneNumber(string phoneNo)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.PhoneNumber.Equals(phoneNo));
        }
        public async Task<IEnumerable<User>> FindByRole(string role)
        {
            return await context.Users.Where(x => x.Role.Equals(role)).ToListAsync();
        }
        public async Task<int> Insert(UserDetailsDTO entity)
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

        public async Task<int> InsertAdministrator(UserDetailsDTO entity)
        {
            try
            {
                Guid UUID = Guid.NewGuid();

                User a = new()
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
                    PasswordExpiryDate = DateTime.Now.AddMonths(6),
                    Role = "ADMINISTRATOR"
                };

                context.Add(a);

                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> Update(UserDetailsDTO entity)
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
        public async Task<int> UpdateAdministrator(UserDetailsDTO entity)
        {
            try
            {
                User a = await FindByUUID(entity.UUID);
                a.UUID = entity.UUID;
                a.FirstName = entity.FirstName;
                a.MiddleName = entity.MiddleName;
                a.LastName = entity.LastName;
                a.Username = entity.Username;
                if (entity.Password != null && !entity.Password.Equals(""))
                {
                    a.Password = entity.Password;
                }
                a.Title = entity.Title;
                a.DateOfBirth = entity.DateOfBirth;
                a.SSN = entity.SSN;
                a.Gender = entity.Gender;
                a.Email = entity.Email;
                a.PhoneNumber = entity.PhoneNumber;

                a.IsDisabled = entity.IsDisabled;
                a.IsExpired = entity.IsExpired;
                a.PasswordExpiryDate = entity.PasswordExpiryDate;

                context.Update(a);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }
        public async Task<int> Delete(UserBasicDTO entity)
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
