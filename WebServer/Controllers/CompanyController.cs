using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.MedicineData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/company")]
    public class CompanyController : Controller<Company>
    {

        public CompanyController(ILogger<CompanyController> logger, IDbService<Company> service) : base(logger, service)
        {
        }

        [HttpGet("all")]
        public override async Task<ActionResult<IEnumerable<Company>>> GetAll()
        {
            var result = await service.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("{id:int}")]
        public override async Task<ActionResult<Company>> GetById(int id)
        {
            var result = await service.FindById(id);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        public override async Task<ActionResult<Company>> GetByUUID(Guid UUID)
        {
            var result = await service.FindByUUID(UUID);

            return result != default ? Ok(result) : NotFound();

        }

        [HttpPost("add")]
        public override async Task<ActionResult<Company>> Add([FromBody] Company company)
        {
            var result = await service.Insert(company);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        public override async Task<ActionResult<Company>> Edit([FromBody] Company company)
        {
            var result = await service.Update(company);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        public override async Task<ActionResult<Company>> Remove(Company company)
        {
            var result = await service.Delete(company);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<ActionResult<Company>> RemoveById(int id)
        {
            var result = await service.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        public override async Task<ActionResult<Company>> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
