using Microsoft.EntityFrameworkCore;
using WebServer.Models.MedicineData;
using WebServer.Models.UserData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class MedicineService : IDbService<Medicine, Medicine, Medicine>
    {
        private readonly MedicineContext context;

        public MedicineService(MedicineContext context)
        {
            this.context = context;
        }

        public async Task<int> Delete(Medicine entity)
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
                Medicine medicine = await FindById(id);
                context.Remove(medicine);
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
                Medicine medicine = await FindByUUID(UUID);
                context.Remove(medicine);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<Medicine>> FindAll()
        {
            return await context.Medicines.Include(m => m.Clearances).Include(m => m.Company).Include(m => m.Issuer).ToListAsync();
        }

        public async Task<Medicine> FindById(int id)
        {
            return await context.Medicines.Include(m => m.Clearances).Include(m => m.Company).Include(m => m.Issuer).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Medicine> FindByUUID(Guid UUID)
        {
            return await context.Medicines.Include(m => m.Clearances).Include(m => m.Company).Include(m => m.Issuer).SingleOrDefaultAsync(m => m.UUID.Equals(UUID));
        }

        public async Task<int> Insert(Medicine entity)
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

        public async Task<int> Update(Medicine entity)
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
