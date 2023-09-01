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
    [Route("api/report")]
    public class ReportController : Controller<Report, ReportDTO, ReportDTO>
    {

        public ReportController(ILogger<ReportController> logger, IDbService<Report, ReportDTO, ReportDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all/all")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> GetAllPaged([FromQuery] string sortOrder, [FromQuery] int? pageNumber, [FromQuery] int pageSize, [FromQuery] Guid? user)
        {
            var result = await ((ReportService)service).FindAllPaged(sortOrder, pageNumber, pageSize, user);

            if (result != null)
            {
                logger.LogInformation("Fetched all report information (Paged).");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Report database empty.");
                return NoContent();
            }
        }

        [HttpGet("count")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> GetCount()
        {
            var result = await ((ReportService)service).Count();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetById(int id)
        {
            var result = await service.FindById(id);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetByUUID(Guid UUID)
        {
            var result = await service.FindByUUID(UUID);

            return result != default ? Ok(result) : NotFound();

        }

        [HttpPost("add")]
        [Authorize(Roles = "DOCTOR,PATIENT")]
        public override async Task<IActionResult> Add([FromBody] ReportDTO entity)
        {
            var result = await service.Insert(entity);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Edit([FromBody] ReportDTO entity)
        {
            var result = await service.Update(entity);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Remove(ReportDTO entity)
        {
            var result = await service.Delete(entity);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> RemoveById(int id)
        {
            var result = await service.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
