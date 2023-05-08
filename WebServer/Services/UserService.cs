using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using WebServer.Models;

namespace WebServer.Services
{
    public sealed class UserService : IMariaDbService<UserModel>
    {
        private readonly UserContext userContext;

        public UserService(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<IEnumerable<UserModel>> FindAll()
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).ToListAsync();
        }
        public async Task<IEnumerable<UserModel>> FindAllDisabled()
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).Where(u => u.IsDisabled).ToListAsync();
        }
        public async Task<IEnumerable<UserModel>> FindAllExpired()
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).Where(u => u.IsExpired).ToListAsync();
        }
        public async Task<UserModel> FindById(int id)
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<UserModel> FindByUUID(Guid UUID)
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.UUID.Equals(UUID));
        }
        public async Task<UserModel> FindByUsername(string username)
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.Username.Equals(username));
        }
        public async Task<UserModel> FindByEmail(string email)
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.Emails.Any(e => e.Email.Equals(email)));
        }
        public async Task<UserModel> FindByPhoneNumber(string phoneNo)
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.PhoneNumbers.Any(e => e.PhoneNumber.Equals(phoneNo)));
        }
        public async Task<UserModel> FindByRole(string role)
        {
            return await userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).SingleOrDefaultAsync(x => x.Roles.Any(e => e.Role.Equals(role)));
        }
        public async Task<int> Insert(UserModel model)
        {
            try
            {
                userContext.Add(model);
                return await userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }
        public async Task<int> Update(UserModel model)
        {
            try
            {
                userContext.Update(model);
                return await userContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }
        public async Task<int> DeleteById(int id)
        {
            try
            {
                UserModel user = await FindById(id);
                userContext.Users.Remove(user);
                return await userContext.SaveChangesAsync();
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
                UserModel user = await FindByUUID(UUID);
                userContext.Users.Remove(user);
                return await userContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }
    }
}
