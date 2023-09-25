using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.Features;
using WebServer.Models.ClinicData;
using WebServer.Models.DTOs;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class ScheduleService : IDbService<Schedule, ScheduleDTO, ScheduleDTO>
    {
        private readonly ScheduleContext context;
        public ScheduleService(ScheduleContext context) 
        {
            this.context = context;
        }

        public async Task<IEnumerable<ScheduleDTO>> FindAll()
        {
            var result = await context.Schedules.Include(r => r.Patient).Include(r => r.Doctor).AsNoTracking().ToListAsync();

            List<ScheduleDTO> results = new List<ScheduleDTO>();

            foreach (var r in result)
            {
                results.Add(new ScheduleDTO
                {
                    Id = r.ScheduleId,
                    DoctorUUID = r.DoctorUUID,
                    PatientUUID = r.PatientUUID,
                    ScheduledDateTime = r.ScheduledDateTime,
                    Event = r.Event
                });
            }

            return results;
        }
        public async Task<PaginatedResultDTO<ScheduleDTO>> FindAllPaged(string sortOrder, int? pageNumber, int pageSize, Guid? patient, Guid? doctor, DateTime? date)
        {
            IQueryable<Schedule> schedules;

            if ((doctor == null || doctor.Equals(Guid.Empty)) && (patient != null || !patient.Equals(Guid.Empty)))
            {
                schedules = context.Schedules.Include(r => r.Doctor).Include(r => r.Patient).Where(r => r.PatientUUID.Equals(patient) && r.ScheduledDateTime > DateTime.Now).AsQueryable();
            }
            else if ((patient == null || patient.Equals(Guid.Empty)) && (doctor != null || !doctor.Equals(Guid.Empty)))
            {
                schedules = context.Schedules.Include(r => r.Doctor).Include(r => r.Patient).Where(r => r.DoctorUUID.Equals(doctor) && r.ScheduledDateTime > DateTime.Now).AsQueryable();
            }
            else
            {
                schedules = null;
            }

            if (schedules != null && date != null)
            {
                schedules = schedules.Where(s => s.ScheduledDateTime.Date.Equals(date.Value.Date)).AsQueryable();
            }

            var result = await PaginatedList<Schedule>.CreateAsync(schedules.AsNoTracking().OrderBy(s => s.ScheduledDateTime), pageNumber ?? 1, pageSize);            

            PaginatedResultDTO<ScheduleDTO> DTOs = new PaginatedResultDTO<ScheduleDTO>();
            DTOs.PageNumber = result.PageIndex;
            DTOs.PageSize = result.PageSize;
            DTOs.TotalPages = result.TotalPages;
            DTOs.TotalItems = result.TotalItems;
            DTOs.HasNext = result.HasNextPage;
            DTOs.HasPrevious = result.HasPreviousPage;

            foreach (var r in result)
            {
                DTOs.items.Add(new ScheduleDTO
                {
                    Id = r.ScheduleId,
                    DoctorUUID = r.DoctorUUID,
                    PatientUUID = r.PatientUUID,
                    ScheduledDateTime = r.ScheduledDateTime,
                    Event = r.Event
                });
            }

            return DTOs;
        }

        public async Task<int> Count(Guid UUID)
        {
            var result = await context.Schedules.Where(r => (r.DoctorUUID.Equals(UUID) || r.PatientUUID.Equals(UUID)) && r.ScheduledDateTime > DateTime.Now).CountAsync();

            return result;
        }

        public async Task<Schedule> FindById(int id)
        {
            return await context.Schedules.SingleOrDefaultAsync(r => r.ScheduleId == id);
        }

        public async Task<ScheduleDTO> FindByIdAsDTO(int id)
        {
            var r = await context.Schedules.SingleOrDefaultAsync(r => r.ScheduleId == id);
            return new ScheduleDTO
            {
                Id = r.ScheduleId,
                DoctorUUID = r.DoctorUUID,
                PatientUUID = r.PatientUUID,
                ScheduledDateTime = r.ScheduledDateTime,
                Event = r.Event
            };
        }

        public Task<ScheduleDTO> FindByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(ScheduleDTO entity)
        {
            Schedule s = new Schedule()
            {
                ScheduleId = 0,
                DoctorUUID = entity.DoctorUUID,
                PatientUUID = entity.PatientUUID, 
                ScheduledDateTime = entity.ScheduledDateTime,
                Event = entity.Event
            };

            context.Schedules.Add(s);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(ScheduleDTO entity)
        {
            Schedule s = await FindById(int.Parse(entity.Id.ToString()));

            s.Event = entity.Event;
            s.ScheduledDateTime = entity.ScheduledDateTime;

            context.Schedules.Update(s);
            return await context.SaveChangesAsync();
        }
        public Task<int> Delete(ScheduleDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
