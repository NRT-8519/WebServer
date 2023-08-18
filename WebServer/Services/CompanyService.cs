using Microsoft.EntityFrameworkCore;
using WebServer.Models.MedicineData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class CompanyService : IDbService<Company, Company, Company>
    {
        private readonly CompanyContext context;
        public CompanyService(CompanyContext context) 
        {
            this.context = context;
        }

        public async Task<int> Delete(Company company)
        {
            try
            {
                context.Companies.Remove(company);
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
                Company company = await FindById(id);
                context.Companies.Remove(company);
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
                Company company = await FindByUUID(UUID);
                context.Companies.Remove(company);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<Company>> FindAll()
        {
            return await context.Companies.ToListAsync();
        }

        public async Task<Company> FindById(int id)
        {
            return await context.Companies.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> FindByUUID(Guid UUID)
        {
            return await context.Companies.SingleOrDefaultAsync(c => c.UUID.Equals(UUID));
        }

        public async Task<int> Insert(Company entity)
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

        public async Task<int> Update(Company entity)
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
