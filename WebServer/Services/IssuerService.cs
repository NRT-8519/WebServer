using Microsoft.EntityFrameworkCore;
using WebServer.Features;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Services.Contexts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebServer.Services
{
    public class IssuerService : IDbService<Issuer, IssuerDTO, IssuerDTO>
    {
        public readonly IssuerContext context;

        public IssuerService(IssuerContext context)
        {
            this.context = context;
        }

        public async Task<int> Count()
        {
            return await context.Issuers.CountAsync();
        }

        public async Task<int> Delete(IssuerDTO entity)
        {
            try
            {
                context.Remove(entity);
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
                Issuer issuer = await FindEntityByUUID(UUID);
                context.Issuers.Remove(issuer);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<IssuerDTO>> FindAll()
        {
            var result = await context.Issuers.ToListAsync();

            List<IssuerDTO> DTOs = new();

            foreach (var issuer in result)
            {
                IssuerDTO i = new()
                {
                    UUID = issuer.UUID,
                    Name = issuer.Name,
                    City = issuer.City,
                    Area = issuer.Area,
                    
                };
                DTOs.Add(i);
            }

            return DTOs;
        }

        public async Task<PaginatedResultDTO<IssuerDTO>> FindAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var issuers = context.Issuers.AsQueryable();

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
                issuers = issuers.Where(c =>
                    c.UUID.ToString().Equals(searchQuery) ||
                    c.Name.Contains(searchQuery) ||
                    c.City.Contains(searchQuery) ||
                    c.Area.Contains(searchQuery)
                );
            }

            var result = await PaginatedList<Issuer>.CreateAsync(issuers.AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<IssuerDTO> DTOs = new()
            {
                PageNumber = result.PageIndex,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalItems = result.TotalItems,
                HasNext = result.HasNextPage,
                HasPrevious = result.HasPreviousPage
            };

            foreach (var issuer in result)
            {
                IssuerDTO c = new()
                {
                    UUID = issuer.UUID,
                    Name = issuer.Name,
                    City = issuer.City,
                    Area = issuer.Area
                };

                DTOs.items.Add(c);
            }

            return DTOs;
        }

        public async Task<Issuer> FindById(int id)
        {
            return await context.Issuers.SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Issuer> FindEntityByUUID(Guid? UUID)
        {
            return await context.Issuers.SingleOrDefaultAsync(i => i.UUID.Equals(UUID));
        }

        public async Task<IssuerDTO> FindByUUID(Guid UUID)
        {
            var issuer = await context.Issuers.SingleOrDefaultAsync(i => i.UUID.Equals(UUID));

            IssuerDTO i = new()
            {
                UUID = issuer.UUID,
                Name = issuer.Name,
                City = issuer.City,
                Area = issuer.Area,

            };

            return i;
        }

        public async Task<int> Insert(IssuerDTO entity)
        {
            try
            {
                Issuer i = new()
                {
                    UUID = Guid.NewGuid(),
                    Name = entity.Name,
                    City = entity.City,
                    Area = entity.Area,
                };

                context.Add(i);

                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> Update(IssuerDTO entity)
        {
            try
            {
                Issuer i = await FindEntityByUUID(entity.UUID);

                i.Name = entity.Name;
                i.City = entity.City;
                i.Area = entity.Area;

                context.Update(i);
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
