using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebServer.Models.ClinicData;
using WebServer.Models.DTOs;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller<Schedule, ScheduleDTO, ScheduleDTO>
    {
        public ScheduleController(ILogger<ScheduleController> logger, IDbService<Schedule, ScheduleDTO, ScheduleDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all/all")]
        public async override Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR,PATIENT,DOCTOR")]
        public async Task<IActionResult> GetAllPaged([FromQuery] string sortOrder, [FromQuery] int? pageNumber, [FromQuery] int pageSize, [FromQuery] Guid? patient, [FromQuery] Guid? doctor, [FromQuery] DateTime? date)
        {
            var result = await ((ScheduleService)service).FindAllPaged(sortOrder, pageNumber, pageSize, patient, doctor, date);

            return Ok(result);
        }

        [HttpGet("count")]
        [Authorize(Roles = "ADMINISTRATOR,PATIENT,DOCTOR")]
        public async Task<IActionResult> GetCount(Guid UUID)
        {
            var result = await ((ScheduleService)service).Count(UUID);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async override Task<IActionResult> GetById(int id)
        {
            var result = await ((ScheduleService)service).FindByIdAsDTO(id);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public async override Task<IActionResult> GetByUUID(Guid UUID)
        {
            var result = await service.FindByUUID(UUID);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpPost("add")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public async override Task<IActionResult> Add([FromBody] ScheduleDTO entity)
        {
            var result = await service.Insert(entity);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> Edit([FromBody] ScheduleDTO entity)
        {
            var result = await service.Update(entity);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public async override Task<IActionResult> Remove([FromBody] ScheduleDTO entity)
        {
            var result = await service.Delete(entity);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public async override Task<IActionResult> RemoveById(int id)
        {
            var result = await service.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public async override Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
