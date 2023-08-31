using Microsoft.EntityFrameworkCore;
using WebServer.Features;
using WebServer.Models.ClinicData;
using WebServer.Models.DTOs;
using WebServer.Services.Contexts;

namespace WebServer.Services
{
    public class NotesService : IDbService<Notes, NotesDTO, NotesDTO>
    {
        private readonly NoteContext context;

        public NotesService(NoteContext context) 
        {
            this.context = context;
        }

        public async Task<IEnumerable<NotesDTO>> FindAll()
        {
            var result = await context.Notes.Include(n => n.Doctor).Include(n => n.Patient).AsNoTracking().ToListAsync();

            List<NotesDTO> notesDTOs = new List<NotesDTO>();

            foreach (var note in result) 
            {
                notesDTOs.Add(new NotesDTO
                {
                    Id = note.Id,
                    DoctorUUID = note.DoctorUUID,
                    PatientUUID = note.PatientUUID,
                    NoteTitle = note.NoteTitle,
                    Note = note.Note,
                    NoteDate = note.NoteDate,
                });
            }

            return notesDTOs;
        }

        public async Task<PaginatedResultDTO<NotesDTO>> FindAllPaged(string sortOrder, int? pageNumber, int pageSize, Guid patient, Guid doctor)
        {
            IQueryable<Notes> requests = context.Notes.Include(r => r.Doctor).Include(r => r.Patient).Where(r => r.PatientUUID.Equals(patient) && r.DoctorUUID.Equals(doctor)).AsQueryable();

            var result = await PaginatedList<Notes>.CreateAsync(requests.OrderByDescending(n => n.NoteDate).AsNoTracking(), pageNumber ?? 1, pageSize);

            PaginatedResultDTO<NotesDTO> DTOs = new PaginatedResultDTO<NotesDTO>();
            DTOs.PageNumber = result.PageIndex;
            DTOs.PageSize = result.PageSize;
            DTOs.TotalPages = result.TotalPages;
            DTOs.TotalItems = result.TotalItems;
            DTOs.HasNext = result.HasNextPage;
            DTOs.HasPrevious = result.HasPreviousPage;

            foreach (var n in result)
            {
                NotesDTO notesDTO = new NotesDTO()
                {
                    Id = n.Id,
                    PatientUUID = n.PatientUUID,
                    DoctorUUID = n.DoctorUUID,
                    NoteTitle = n.NoteTitle,
                    Note = n.Note,
                    NoteDate = n.NoteDate,
                    
                };
                DTOs.items.Add(notesDTO);
            }

            return DTOs;
        }

        public async Task<Notes> FindById(int id)
        {
            return await context.Notes.Include(n => n.Doctor).Include(n => n.Patient).AsNoTracking().SingleOrDefaultAsync(n => n.Id == id);
        }

        public async Task<NotesDTO> FindByIdAsDTO(int id)
        {
            var result = await context.Notes.Include(n => n.Doctor).Include(n => n.Patient).SingleOrDefaultAsync(n => n.Id == id);

            return new NotesDTO
            {
                Id = result.Id,
                DoctorUUID = result.DoctorUUID,
                PatientUUID = result.PatientUUID,
                NoteTitle = result.NoteTitle,
                Note = result.Note,
                NoteDate = result.NoteDate,
            };
        }

        public async Task<NotesDTO> FindByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(NotesDTO entity)
        {
            Notes n = new Notes
            {
                Id = 0,
                DoctorUUID = entity.DoctorUUID,
                PatientUUID = entity.PatientUUID,
                NoteTitle = entity.NoteTitle,
                Note = entity.Note,
                NoteDate = DateTime.Now,
            };

            context.Notes.Add(n);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(NotesDTO entity)
        {
            var result = await context.Notes.Include(n => n.Doctor).Include(n => n.Patient).AsNoTracking().SingleOrDefaultAsync(n => n.Id.Equals(entity.Id));

            result.NoteTitle = entity.NoteTitle;
            result.Note = entity.Note;
            result.NoteDate = DateTime.Now;

            context.Notes.Update(result);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(NotesDTO entity)
        {
            var result = await context.Notes.Include(n => n.Doctor).Include(n => n.Patient).AsNoTracking().SingleOrDefaultAsync(n => n.Id.Equals(entity.Id));

            context.Notes.Remove(result);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteById(int id)
        {
            var result = await context.Notes.Include(n => n.Doctor).Include(n => n.Patient).AsNoTracking().SingleOrDefaultAsync(n => n.Id.Equals(id));

            context.Notes.Remove(result);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
