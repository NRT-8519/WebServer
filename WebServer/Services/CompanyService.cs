using Microsoft.EntityFrameworkCore;
using WebServer.Features;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class CompanyService : IDbService<Company, CompanyDTO, CompanyDTO>
    {
        private readonly CompanyContext context;
        public CompanyService(CompanyContext context) 
        {
            this.context = context;
        }

        public async Task<int> Count()
        {
            return await context.Companies.CountAsync();
        }

        public async Task<int> Delete(CompanyDTO company)
        {
            try
            {
                context.Remove(company);
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
                Company company = await FindEntityByUUID(UUID);
                context.Companies.Remove(company);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<CompanyDTO>> FindAll()
        {
            var result = await context.Companies.ToListAsync();

            List<CompanyDTO> DTOs = new();

            foreach (var company in result)
            {
                CompanyDTO c = new()
                {
                    UUID = company.UUID,
                    Name = company.Name,
                    Country = company.Country,
                    City = company.City,
                    Address = company.Address
                };
                DTOs.Add(c);
            }

            return DTOs;
        }

        public async Task<PaginatedResultDTO<CompanyDTO>> FindAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var companies = context.Companies.AsQueryable();

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
                companies = companies.Where(c =>
                    c.UUID.ToString().Equals(searchQuery) ||
                    c.Name.Contains(searchQuery) ||
                    c.City.Contains(searchQuery) ||
                    c.Country.Contains(searchQuery)
                );
            }

            var result = await PaginatedList<Company>.CreateAsync(companies.AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<CompanyDTO> DTOs = new()
            {
                PageNumber = result.PageIndex,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalItems = result.TotalItems,
                HasNext = result.HasNextPage,
                HasPrevious = result.HasPreviousPage
            };

            foreach (var company in result) 
            {
                CompanyDTO c = new() 
                { 
                    UUID = company.UUID, 
                    Name = company.Name, 
                    Country = company.Country, 
                    City = company.City, 
                    Address = company.Address 
                };

                DTOs.items.Add(c);
            }

            return DTOs;
        }

        public async Task<Company> FindById(int id)
        {
            return await context.Companies.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company> FindEntityByUUID(Guid? UUID)
        {
            return await context.Companies.SingleOrDefaultAsync(c => c.UUID.Equals(UUID));
        }

        public async Task<CompanyDTO> FindByUUID(Guid UUID)
        {
            var company = await context.Companies.SingleOrDefaultAsync(c => c.UUID.Equals(UUID));

            CompanyDTO c = new()
            {
                UUID = company.UUID,
                Name = company.Name,
                Country = company.Country,
                City = company.City,
                Address = company.Address
            };

            return c;
        }

        public async Task<int> Insert(CompanyDTO entity)
        {
            try
            {
                Company c = new()
                {
                    UUID = Guid.NewGuid(),
                    Name = entity.Name,
                    Country = entity.Country,
                    City = entity.City,
                    Address = entity.Address
                };

                context.Add(c);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> Update(CompanyDTO entity)
        {
            try
            {
                Company c = await FindEntityByUUID(entity.UUID);

                c.Name = entity.Name;
                c.Country = entity.Country;
                c.City = entity.City;
                c.Address = entity.Address;

                context.Update(c);
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
