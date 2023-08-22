using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.UserData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public sealed class UserService : IDbService<User, User, User>
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
