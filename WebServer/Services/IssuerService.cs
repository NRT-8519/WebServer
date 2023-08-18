using Microsoft.EntityFrameworkCore;
using WebServer.Models.MedicineData;
using WebServer.Services.Contexts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebServer.Services
{
    public class IssuerService : IDbService<Issuer, Issuer, Issuer>
    {
        public readonly IssuerContext context;

        public IssuerService(IssuerContext context)
        {
            this.context = context;
        }

        public async Task<int> Delete(Issuer entity)
        {
            try
            {
                context.Issuers.Remove(entity);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteById(int id)
        {
            try
            {
                Issuer issuer = await FindById(id);
                context.Issuers.Remove(issuer);
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
                Issuer issuer = await FindByUUID(UUID);
                context.Issuers.Remove(issuer);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<Issuer>> FindAll()
        {
            return await context.Issuers.ToListAsync();
        }

        public async Task<Issuer> FindById(int id)
        {
            return await context.Issuers.SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Issuer> FindByUUID(Guid UUID)
        {
            return await context.Issuers.SingleOrDefaultAsync(i => i.UUID.Equals(UUID));
        }

        public async Task<int> Insert(Issuer entity)
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

        public async Task<int> Update(Issuer entity)
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
    }
}
