using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using WebServer.Features;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Models.UserData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class MedicineService : IDbService<Medicine, MedicineDTO, MedicineDTO>
    {
        private readonly MedicineContext context;
        private readonly CompanyContext companyContext;
        private readonly IssuerContext issuerContext;

        public MedicineService(MedicineContext context, CompanyContext companyContext, IssuerContext issuerContext)
        {
            this.context = context;
            this.companyContext = companyContext;
            this.issuerContext = issuerContext;
        }

        public async Task<int> Delete(MedicineDTO entity)
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

        public async Task<int> DeleteByUUID(Guid UUID)
        {
            try
            {
                Medicine medicine = await FindEntityByUUID(UUID);
                context.Remove(medicine);
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<MedicineDTO>> FindAll()
        {
            var result = await context.Medicines.Include(m => m.Clearance).Include(m => m.Company).Include(m => m.Issuer).ToListAsync();

            List<MedicineDTO> DTOs = new();

            foreach (var medicine in result)
            {
                MedicineDTO m = new()
                {
                    UUID = medicine.UUID,
                    Name = medicine.Name,
                    Type = medicine.Type,
                    Dosage = medicine.Dosage,
                    DosageType = medicine.DosageType,
                    EAN = medicine.EAN,
                    ATC = medicine.ATC,
                    UniqueClassification = medicine.UniqueClassification,
                    INN = medicine.INN,
                    PrescriptionType = medicine.PrescriptionType,
                    Company = new()
                    {
                        UUID = medicine.Company.UUID,
                        Name = medicine.Company.Name,
                        Country = medicine.Company.Country,
                        City = medicine.Company.City,
                        Address = medicine.Company.Address
                    },
                    Issuer = new()
                    {
                        UUID = medicine.Issuer.UUID,
                        Name = medicine.Issuer.Name,
                        City = medicine.Issuer.City,
                        Area = medicine.Issuer.Area
                    },
                    Clearance = new()
                    {
                        UUID = medicine.Clearance.UUID,
                        ClearanceNumber = medicine.Clearance.ClearanceNumber,
                        BeginDate = medicine.Clearance.BeginDate,
                        ExpiryDate = medicine.Clearance.ExpiryDate,
                    }
                };
                DTOs.Add(m);
            }

            return DTOs;
        }

        public async Task<PaginatedResultDTO<MedicineDTO>> FindAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize, Guid? company, Guid? issuer)
        {
            IQueryable<Medicine> medicines;

            if ((company == null || company.Equals(Guid.Empty)) && (issuer == null || issuer.Equals(Guid.Empty)))
            {
                medicines = context.Medicines.Include(m => m.Clearance).Include(m => m.Company).Include(m => m.Issuer).AsQueryable();
            }
            else if (company != null && !company.Equals(Guid.Empty))
            {
                medicines = context.Medicines.Include(m => m.Clearance).Include(m => m.Company).Include(m => m.Issuer).Where(m => m.Company.UUID.Equals(company)).AsQueryable();
            }
            else if (issuer != null && !issuer.Equals(Guid.Empty))
            {
                medicines = context.Medicines.Include(m => m.Clearance).Include(m => m.Company).Include(m => m.Issuer).Where(m => m.Issuer.UUID.Equals(issuer)).AsQueryable();
            }
            else
            {
                medicines = context.Medicines.Include(m => m.Clearance).Include(m => m.Company).Include(m => m.Issuer).Where(m => (m.Issuer.UUID.Equals(issuer) && m.Company.UUID.Equals(company))).AsQueryable();
            }

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
                medicines = medicines.Where(a =>
                    a.Name.Contains(searchQuery) ||
                    a.Type.Contains(searchQuery) ||
                    a.Dosage.Contains(searchQuery) ||
                    a.DosageType.Contains(searchQuery) ||
                    a.EAN.Contains(searchQuery) ||
                    a.ATC.Contains(searchQuery) ||
                    a.UniqueClassification.Contains(searchQuery) ||
                    a.INN.ToString().Contains(searchQuery) ||
                    a.PrescriptionType.ToString().Contains(searchQuery) ||
                    a.Issuer.Name.Contains(searchQuery) ||
                    a.Company.Name.Contains(searchQuery)
                );
            }

            var result = await PaginatedList<Medicine>.CreateAsync(medicines.AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<MedicineDTO> DTOs = new();
            DTOs.PageNumber = result.PageIndex;
            DTOs.PageSize = result.PageSize;
            DTOs.TotalPages = result.TotalPages;
            DTOs.TotalItems = result.TotalItems;
            DTOs.HasNext = result.HasNextPage;
            DTOs.HasPrevious = result.HasPreviousPage;

            foreach (var medicine in result)
            {
                MedicineDTO m = new()
                {
                    UUID = medicine.UUID,
                    Name = medicine.Name,
                    Type = medicine.Type,
                    Dosage = medicine.Dosage,
                    DosageType = medicine.DosageType,
                    EAN = medicine.EAN,
                    ATC = medicine.ATC,
                    UniqueClassification = medicine.UniqueClassification,
                    INN = medicine.INN,
                    PrescriptionType = medicine.PrescriptionType,
                    Company = new()
                    {
                        UUID = medicine.Company.UUID,
                        Name = medicine.Company.Name,
                        Country = medicine.Company.Country,
                        City = medicine.Company.City,
                        Address = medicine.Company.Address
                    },
                    Issuer = new()
                    {
                        UUID = medicine.Issuer.UUID,
                        Name = medicine.Issuer.Name,
                        City = medicine.Issuer.City,
                        Area = medicine.Issuer.Area
                    },
                    Clearance = new()
                    {
                        UUID = medicine.Clearance.UUID,
                        ClearanceNumber = medicine.Clearance.ClearanceNumber,
                        BeginDate = medicine.Clearance.BeginDate,
                        ExpiryDate = medicine.Clearance.ExpiryDate,
                    }
                };
                DTOs.items.Add(m);
            }

            return DTOs;
        }

        public async Task<int> Count()
        {
            return await context.Medicines.CountAsync();
        }

        public async Task<Medicine> FindEntityByUUID(Guid? UUID)
        {
            return await context.Medicines.Include(m => m.Clearance).Include(m => m.Company).Include(m => m.Issuer).SingleOrDefaultAsync(m => m.UUID.Equals(UUID));
        }

        public async Task<MedicineDTO> FindByUUID(Guid UUID)
        {
            var result = await context.Medicines.Include(m => m.Clearance).Include(m => m.Company).Include(m => m.Issuer).SingleOrDefaultAsync(m => m.UUID.Equals(UUID));

            return new()
            {
                UUID = result.UUID,
                Name = result.Name,
                Type = result.Type,
                Dosage = result.Dosage,
                DosageType = result.DosageType,
                EAN = result.EAN,
                ATC = result.ATC,
                UniqueClassification = result.UniqueClassification,
                INN = result.INN,
                PrescriptionType = result.PrescriptionType,
                Company = new()
                {
                    UUID = result.Company.UUID,
                    Name = result.Company.Name,
                    Country = result.Company.Country,
                    City = result.Company.City,
                    Address = result.Company.Address
                },
                Issuer = new()
                {
                    UUID = result.Issuer.UUID,
                    Name = result.Issuer.Name,
                    City = result.Issuer.City,
                    Area = result.Issuer.Area
                },
                Clearance = new()
                {
                    UUID = result.Clearance.UUID,
                    ClearanceNumber = result.Clearance.ClearanceNumber,
                    BeginDate = result.Clearance.BeginDate,
                    ExpiryDate = result.Clearance.ExpiryDate,
                }
            };
        }

        public async Task<int> Insert(MedicineDTO entity)
        {
            try
            {
                var company = await companyContext.Companies.SingleOrDefaultAsync(x => x.UUID.Equals(entity.Company.UUID));
                var issuer = await issuerContext.Issuers.SingleOrDefaultAsync(x => x.UUID.Equals(entity.Issuer.UUID));

                Medicine m = new()
                {
                    UUID = Guid.NewGuid(),
                    Name = entity.Name,
                    Type = entity.Type,
                    Dosage = entity.Dosage,
                    DosageType = entity.DosageType,
                    EAN = entity.EAN,
                    ATC = entity.ATC,
                    UniqueClassification = entity.UniqueClassification,
                    INN = entity.INN,
                    PrescriptionType = entity.PrescriptionType,
                    CompanyUUID = company.UUID,
                    IssuerUUID = issuer.UUID,
                    Clearance = new()
                    {
                        UUID = Guid.NewGuid(),
                        ClearanceNumber = entity.Clearance.ClearanceNumber,
                        BeginDate = entity.Clearance.BeginDate,
                        ExpiryDate = entity.Clearance.ExpiryDate,
                    },
                };
                context.Add(m);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public async Task<int> Update(MedicineDTO entity)
        {
            try
            {
                Medicine m = await FindEntityByUUID(entity.UUID);

                m.Name = entity.Name;
                m.Type = entity.Type;
                m.Dosage = entity.Dosage;
                m.DosageType = entity.DosageType;
                m.EAN = entity.EAN;
                m.ATC = entity.ATC;
                m.UniqueClassification = entity.UniqueClassification;
                m.INN = entity.INN;
                m.PrescriptionType = entity.PrescriptionType;

                //Company c = await companyContext.FindAsync<Company>(entity.Company.UUID);
                //Issuer i = await issuerContext.FindAsync<Issuer>(entity.Issuer.UUID);
                m.CompanyUUID = entity.Company.UUID;
                //m.Company = c;

                m.IssuerUUID = entity.Issuer.UUID;
                //m.Issuer = i;

                m.Clearance.BeginDate = entity.Clearance.BeginDate;
                m.Clearance.ExpiryDate = entity.Clearance.ExpiryDate;

                context.Update(m);
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Console.WriteLine(exception.ToString());
                return 0;
            }
        }

        public Task<Medicine> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
