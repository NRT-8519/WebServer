using Microsoft.EntityFrameworkCore;
using WebServer.Features;
using WebServer.Models.ClinicData;
using WebServer.Models.DTOs;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class PrescriptionService : IDbService<Prescription, PrescriptionDTO, PrescriptionDTO>
    {
        private readonly PrescriptionContext context;
        public PrescriptionService(PrescriptionContext context) { this.context = context; }

        public async Task<IEnumerable<PrescriptionDTO>> FindAll()
        {
            var result = await context.Prescriptions.Include(p => p.Doctor).Include(p => p.Patient).Include(p => p.Medicine).AsNoTracking().ToListAsync();
            
            List<PrescriptionDTO> results = new List<PrescriptionDTO>();

            foreach (var r in result)
            {
                results.Add(new PrescriptionDTO()
                {
                    Id = r.Id,
                    Doctor = r.Doctor.FirstName + " " + r.Doctor.LastName,
                    Patient = r.Patient.FirstName + " " + r.Patient.LastName,
                    Medicine = r.Medicine.Name,
                    Administered = r.DateAdministered,
                    Prescribed = r.DatePrescribed,
                    Notes = r.PrescriptionNotes
                });
            }

            return results;
        }

        public async Task<PaginatedResultDTO<PrescriptionDTO>> FindAllPaged(string sortOrder, int? pageNumber, int pageSize, Guid? doctor, Guid? patient)
        {
            IQueryable<Prescription> prescriptions;

            if ((doctor == null || doctor.Equals(Guid.Empty)) && (patient != null || !patient.Equals(Guid.Empty)))
            {
                prescriptions = context.Prescriptions.Include(r => r.Doctor).Include(r => r.Patient).Include(r => r.Medicine).Where(r => r.PatientUUID.Equals(patient)).AsQueryable();
            }
            else if ((patient == null || patient.Equals(Guid.Empty)) && (doctor != null || !doctor.Equals(Guid.Empty)))
            {
                prescriptions = context.Prescriptions.Include(r => r.Doctor).Include(r => r.Patient).Include(r => r.Medicine).Where(r => r.DoctorUUID.Equals(doctor)).AsQueryable();
            }
            else
            {
                return null;
            }

            var result = await PaginatedList<Prescription>.CreateAsync(prescriptions.AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<PrescriptionDTO> DTOs = new PaginatedResultDTO<PrescriptionDTO>(); ;

            DTOs.PageNumber = result.PageIndex;
            DTOs.PageSize = result.PageSize;
            DTOs.TotalPages = result.TotalPages;
            DTOs.TotalItems = result.TotalItems;
            DTOs.HasNext = result.HasNextPage;
            DTOs.HasPrevious = result.HasPreviousPage;

            foreach (var r in result)
            {
                DTOs.items.Add(new PrescriptionDTO()
                {
                    Id = r.Id,
                    Doctor = r.Doctor.FirstName + " " + r.Doctor.LastName,
                    Patient = r.Patient.FirstName + " " + r.Patient.LastName,
                    Medicine = r.Medicine.Name,
                    Administered = r.DateAdministered,
                    Prescribed = r.DatePrescribed,
                    Notes = r.PrescriptionNotes
                });
            }

            return DTOs;
        }

        public async Task<int> Count(Guid UUID)
        {
            var result = await context.Prescriptions.Where(r => r.PatientUUID.Equals(UUID) || r.DoctorUUID.Equals(UUID)).CountAsync();

            return result;
        }

        public async Task<PrescriptionDTO> FindByIdAsDTO(int id)
        {
            var r = await context.Prescriptions.SingleOrDefaultAsync(p => p.Id == id);

            return new PrescriptionDTO()
            {
                Id = r.Id,
                Doctor = r.Doctor.FirstName + " " + r.Doctor.LastName,
                Patient = r.Patient.FirstName + " " + r.Patient.LastName,
                Medicine = r.Medicine.Name,
                Administered = r.DateAdministered,
                Prescribed = r.DatePrescribed,
                Notes = r.PrescriptionNotes
            };
        }

        public async Task<Prescription> FindById(int id)
        {
            return await context.Prescriptions.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> Insert(PrescriptionDTO entity)
        {
            Prescription p = new Prescription()
            {
                Id = 0,
                DoctorUUID = Guid.Parse(entity.Doctor),
                PatientUUID = Guid.Parse(entity.Patient),
                MedicineUUID = Guid.Parse(entity.Medicine),
                DatePrescribed = entity.Prescribed,
                DateAdministered = entity.Administered,
                PrescriptionNotes = entity.Notes
            };

            context.Prescriptions.Add(p);

            return await context.SaveChangesAsync();
        }

        public Task<PrescriptionDTO> FindByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(PrescriptionDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(PrescriptionDTO entity)
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
