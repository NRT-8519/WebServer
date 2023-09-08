using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebServer.Features;
using WebServer.Models;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class RequestService : IDbService<Request, RequestDTO, RequestDTO>
    {
        private readonly RequestContext context;

        public RequestService(RequestContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RequestDTO>> FindAll()
        {
            var result = await context.Requests.Include(r => r.Patient).Include(r => r.Doctor).AsNoTracking().ToListAsync();

            List<RequestDTO> results = new List<RequestDTO>();

            foreach (var r in result)
            {
                results.Add(new RequestDTO
                {
                    UUID = r.UUID,
                    Patient = r.Patient.FirstName + " " + r.Patient.LastName,
                    Doctor = r.Doctor.FirstName + " " + r.Doctor.LastName,
                    Title = r.Title,
                    Description = r.Description,
                    Type = r.Type,
                    Status = r.Status,
                    Reason = r.Reason,
                    RequestDate = r.RequestDate,
                });
            }

            return results;
        }

        public async Task<int> Count(string status)
        {
            var result = await context.Requests.Where(r => r.Status.Equals(status)).CountAsync();

            return result;
        }

        public async Task<int> Count(Guid UUID, string status)
        {
            var result = await context.Requests.Where(r => (r.DoctorUUID.Equals(UUID) || r.PatientUUID.Equals(UUID)) && r.Status.Equals(status)).CountAsync();

            return result;
        }

        public async Task<PaginatedResultDTO<RequestDTO>> FindAllPaged(string sortOrder, int? pageNumber, int pageSize, Guid? patient, Guid? doctor)
        {
            IQueryable<Request> requests;

            if ((doctor == null || doctor.Equals(Guid.Empty)) && (patient != null || !patient.Equals(Guid.Empty)))
            {
                requests = context.Requests.Include(r => r.Doctor).Include(r => r.Patient).Where(r => r.PatientUUID.Equals(patient)).AsQueryable();
            }
            else if ((patient == null || patient.Equals(Guid.Empty)) && (doctor != null || !doctor.Equals(Guid.Empty)))
            {
                requests = context.Requests.Include(r => r.Doctor).Include(r => r.Patient).Where(r => r.DoctorUUID.Equals(doctor)).AsQueryable();
            }
            else if ((patient != null || !patient.Equals(Guid.Empty)) && (doctor != null || !doctor.Equals(Guid.Empty)))
            {
                requests = context.Requests.Include(r => r.Doctor).Include(r => r.Patient).Where(r => r.DoctorUUID.Equals(doctor) && r.PatientUUID.Equals(patient)).AsQueryable();
            }
            else
            {
                requests = null;
            }

            var result = await PaginatedList<Request>.CreateAsync(requests.AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<RequestDTO> DTOs = new PaginatedResultDTO<RequestDTO>();
            DTOs.PageNumber = result.PageIndex;
            DTOs.PageSize = result.PageSize;
            DTOs.TotalPages = result.TotalPages;
            DTOs.TotalItems = result.TotalItems;
            DTOs.HasNext = result.HasNextPage;
            DTOs.HasPrevious = result.HasPreviousPage;

            foreach (var r in result) 
            {
                RequestDTO requestDTO = new RequestDTO()
                {
                    UUID = r.UUID,
                    Patient = r.Patient.FirstName + " " + r.Patient.LastName,
                    Doctor = r.Doctor.FirstName + " " + r.Doctor.LastName,
                    Title = r.Title,
                    Description = r.Description,
                    Type = r.Type,
                    Status = r.Status,
                    Reason = r.Reason,
                    RequestDate = r.RequestDate,
                };
                DTOs.items.Add(requestDTO);
            }

            return DTOs;
        }

        public Task<Request> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestDTO> FindByUUID(Guid UUID)
        {
            var r = await context.Requests.Include(r => r.Doctor).Include(r => r.Patient).SingleOrDefaultAsync(r => r.UUID.Equals(UUID));

            RequestDTO requestDTO = new RequestDTO()
            {
                UUID = r.UUID,
                Patient = r.Patient.FirstName + " " + r.Patient.LastName,
                Doctor = r.Doctor.FirstName + " " + r.Doctor.LastName,
                Title = r.Title,
                Description = r.Description,
                Type = r.Type,
                Status = r.Status,
                Reason = r.Reason,
                RequestDate = r.RequestDate
            };

            return requestDTO;
        }

        public async Task<int> Insert([FromBody] RequestDTO entity)
        {
            Request request = new Request()
            {
                UUID = Guid.NewGuid(),
                PatientUUID = Guid.Parse(entity.Patient),
                DoctorUUID = Guid.Parse(entity.Doctor),
                Title = entity.Title,
                Description = entity.Description,
                Type = entity.Type,
                Status = "AWAITING",
                Reason = "",
                RequestDate = DateTime.Now,
            };

            await context.Requests.AddAsync(request);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update([FromBody] RequestDTO entity)
        {
            var r = await context.Requests.Include(r => r.Doctor).Include(r => r.Patient).SingleOrDefaultAsync(r => r.UUID.Equals(entity.UUID));

            r.Status = entity.Status;
            r.Reason = entity.Reason;

            context.Requests.Update(r);
            return await context.SaveChangesAsync();
        }
        public async Task<int> Delete([FromBody] RequestDTO entity)
        {
            var r = await context.Requests.Include(r => r.Doctor).Include(r => r.Patient).SingleOrDefaultAsync(r => r.UUID.Equals(entity.UUID));

            context.Requests.Remove(r);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteByUUID(Guid UUID)
        {
            var r = await context.Requests.Include(r => r.Doctor).Include(r => r.Patient).SingleOrDefaultAsync(r => r.UUID.Equals(UUID));

            context.Requests.Remove(r);
            return await context.SaveChangesAsync();
        }
    }
}
