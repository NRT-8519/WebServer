using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/request")]
    public class RequestController : Controller<Request, RequestDTO, RequestDTO>
    {

        public RequestController(ILogger<RequestController> logger, IDbService<Request, RequestDTO, RequestDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all/all")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR,PATIENT,DOCTOR")]
        public async Task<IActionResult> GetAllPaged([FromQuery] string sortOrder, [FromQuery] int? pageNumber, [FromQuery] int pageSize, [FromQuery] Guid? patient, [FromQuery] Guid? doctor)
        {
            var result = await ((RequestService)service).FindAllPaged(sortOrder, pageNumber, pageSize, patient, doctor);

            return Ok(result);
        }

        [HttpGet("count")]
        [Authorize(Roles = "ADMINISTRATOR,PATIENT,DOCTOR")]
        public async Task<IActionResult> GetCount(string status)
        {
            var result = await ((RequestService)service).Count(status);

            return Ok(result);
        }

        [HttpGet("count/{UUID}/{status}")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public async Task<IActionResult> GetCount(Guid UUID, string status)
        {
            var result = await ((RequestService)service).Count(UUID, status);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public override async Task<IActionResult> GetById(int id)
        {
            var result = await service.FindById(id);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public override async Task<IActionResult> GetByUUID(Guid UUID)
        {
            var result = await service.FindByUUID(UUID);

            return result != default ? Ok(result) : NotFound();

        }

        [HttpPost("add")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public override async Task<IActionResult> Add([FromBody] RequestDTO Request)
        {
            var result = await service.Insert(Request);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        [Authorize(Roles = "DOCTOR")]
        public override async Task<IActionResult> Edit([FromBody] RequestDTO Request)
        {
            var result = await service.Update(Request);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public override async Task<IActionResult> Remove(RequestDTO Request)
        {
            var result = await service.Delete(Request);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<IActionResult> RemoveById(int id)
        {
            var result = await service.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public override async Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
