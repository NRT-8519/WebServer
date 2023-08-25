using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.DTOs;
using WebServer.Models.MedicineData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/company")]
    public class CompanyController : Controller<Company, CompanyDTO, CompanyDTO>
    {

        public CompanyController(ILogger<CompanyController> logger, IDbService<Company, CompanyDTO, CompanyDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all/all")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var result = await ((CompanyService)service).FindAllPaged(sortOrder, searchQuery, currentFilter, pageNumber, pageSize);

            if (result.items.Any())
            {
                logger.LogInformation("Fetched all companies information (Paged).");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Companies database empty.");
                return NoContent();
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var result = await ((CompanyService)service).Count();

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

            return result != default ? Ok(result) : NotFound();

        }

        [HttpPost("add")]
        public override async Task<IActionResult> Add([FromBody] CompanyDTO company)
        {
            var result = await service.Insert(company);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        public override async Task<IActionResult> Edit([FromBody] CompanyDTO company)
        {
            var result = await service.Update(company);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        public override async Task<IActionResult> Remove(CompanyDTO company)
        {
            var result = await service.Delete(company);

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
