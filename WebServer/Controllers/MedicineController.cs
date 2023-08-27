using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/medicine")]
    [ApiController]
    public class MedicineController : Controller<Medicine, MedicineDTO, MedicineDTO>
    {
        public MedicineController(ILogger<MedicineController> logger, IDbService<Medicine, MedicineDTO, MedicineDTO> service) : base(logger, service) { }

        [HttpGet("all/all")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();

            if (result.Any())
            {
                logger.LogInformation("Fetched all medicine information (Paged).");
                return Ok(result);
            }
            else
            {

                logger.LogInformation("Patients database empty.");
                return NoContent();
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize, Guid? company, Guid? issuer)
        {
            var result = await ((MedicineService) service).FindAllPaged(sortOrder, searchQuery, currentFilter, pageNumber, pageSize, company, issuer);

            if (result.items.Any())
            {
                logger.LogInformation("Fetched all medicine information (Paged).");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Medicine database empty.");
                return NoContent();
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var result = await ((MedicineService) service).Count();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public override async Task<IActionResult> GetById(int id)
        {
            var result = await service.FindById(id);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        public override async Task<IActionResult> GetByUUID(Guid UUID)
        {
            var result = await service.FindByUUID(UUID);

            if (result != default)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("add")]
        public override async Task<IActionResult> Add([FromBody] MedicineDTO medicine)
        {
            var result = await service.Insert(medicine);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        public override async Task<IActionResult> Edit([FromBody] MedicineDTO medicine)
        {
            var result = await service.Update(medicine);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        public override async Task<IActionResult> Remove(MedicineDTO medicine)
        {
            var result = await service.Delete(medicine);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<IActionResult> RemoveById(int id)
        {
            var result = await service.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        public override async Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
