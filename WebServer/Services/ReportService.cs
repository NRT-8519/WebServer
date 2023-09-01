using Microsoft.EntityFrameworkCore;
using System;
using WebServer.Features;
using WebServer.Models;
using WebServer.Models.ClinicData;
using WebServer.Models.DTOs;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class ReportService : IDbService<Report, ReportDTO, ReportDTO>
    {
        private readonly ReportContext context;

        public ReportService(ReportContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ReportDTO>> FindAll()
        {
            var result = await context.Reports.Include(r => r.User).ToListAsync();

            List<ReportDTO> results = new List<ReportDTO>();
            foreach (var report in result)
            {
                results.Add(new ReportDTO()
                {
                    UUID = report.UUID,
                    ReportedBy = report.UserUUID,
                    Title = report.Title,
                    Description = report.Description,
                    Date = report.Date
                });
            }

            return results;
        }

        public async Task<PaginatedResultDTO<ReportDTO>> FindAllPaged(string sortOrder, int? pageNumber, int pageSize, Guid? user)
        {
            IQueryable<Report> reports;

            if (user != null)
            {
                reports = context.Reports.Include(r => r.User).Where(r => r.UserUUID.Equals(user)).AsQueryable();
            }
            else
            {
                reports = context.Reports.Include(r => r.User).AsQueryable();
            }

            var result = await PaginatedList<Report>.CreateAsync(reports.AsNoTracking().OrderByDescending(s => s.Date), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<ReportDTO> DTOs = new PaginatedResultDTO<ReportDTO>();
            DTOs.PageNumber = result.PageIndex;
            DTOs.PageSize = result.PageSize;
            DTOs.TotalPages = result.TotalPages;
            DTOs.TotalItems = result.TotalItems;
            DTOs.HasNext = result.HasNextPage;
            DTOs.HasPrevious = result.HasPreviousPage;

            foreach (var r in result)
            {
                DTOs.items.Add(new ReportDTO
                {
                    UUID = r.UUID,
                    ReportedBy = r.UserUUID,
                    Title = r.Title,
                    Description = r.Description,
                    Date = r.Date,
                });
            }

            return DTOs;
        }

        public async Task<int> Count()
        {
            return await context.Reports.CountAsync();
        }

        public async Task<Report> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ReportDTO> FindByUUID(Guid UUID)
        {
            var report = await context.Reports.Include(r => r.User).SingleOrDefaultAsync(r => r.UUID.Equals(UUID));

            return new ReportDTO()
            {
                UUID = report.UUID,
                ReportedBy = report.UserUUID,
                Title = report.Title,
                Description = report.Description,
                Date = report.Date
            };
        }

        public async Task<int> Insert(ReportDTO entity)
        {
            Report r = new Report()
            {
                UUID = Guid.NewGuid(),
                UserUUID = entity.ReportedBy,
                Title = entity.Title,
                Description = entity.Description,
                Date = DateTime.Now,
            };

            context.Reports.Add(r);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(ReportDTO entity)
        {
            var report = await context.Reports.Include(r => r.User).SingleOrDefaultAsync(r => r.UUID.Equals(entity.UUID));

            report.Title = entity.Title;
            report.Description = entity.Description;
            report.Date = entity.Date;

            context.Reports.Update(report);
            return await context.SaveChangesAsync();
        }
        public async Task<int> Delete(ReportDTO entity)
        {
            var report = await context.Reports.Include(r => r.User).SingleOrDefaultAsync(r => r.UUID.Equals(entity.UUID));
            
            context.Reports.Remove(report);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteByUUID(Guid UUID)
        {
            var report = await context.Reports.Include(r => r.User).SingleOrDefaultAsync(r => r.UUID.Equals(UUID));

            context.Reports.Remove(report);
            return await context.SaveChangesAsync();
        }
    }
}
