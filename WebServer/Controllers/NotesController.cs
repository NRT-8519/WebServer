using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.ClinicData;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/notes")]
    [ApiController]
    public class NotesController : Controller<Notes, NotesDTO, NotesDTO>
    {
        public NotesController(ILogger<NotesController> logger, IDbService<Notes, NotesDTO, NotesDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all/all")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("all")]
        [Authorize(Roles = "DOCTOR")]
        public async Task<IActionResult> GetAllPaged(string sortOrder, int? pageNumber, int pageSize, Guid patient, Guid doctor)
        {
            var result = await ((NotesService) service).FindAllPaged(sortOrder, pageNumber, pageSize, patient, doctor);
            return result != null ? Ok(result) : NoContent();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> GetById(int id)
        {
            var result = await ((NotesService)service).FindByIdAsDTO(id);

            return result != null ? Ok(result) : NoContent();
        }

        [HttpGet("{UUID}")]
        [Authorize(Roles = "DOCTOR")]
        public override Task<IActionResult> GetByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }

        [HttpPost("add")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> Add(NotesDTO entity)
        {
            var result = await service.Insert(entity);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> Edit(NotesDTO entity)
        {
            var result = await service.Update(entity);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> Remove(NotesDTO entity)
        {
            var result = await service.Delete(entity);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> RemoveById(int id)
        {
            var result = await service.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
