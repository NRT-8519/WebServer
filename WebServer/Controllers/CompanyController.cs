using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/company")]
    public class CompanyController : Controller<Company>
    {
        private readonly ILogger<CompanyController> logger;
        private readonly IDbService<Company> companyService;

        public CompanyController(ILogger<CompanyController> logger, IDbService<Company> companyService)
        {
            this.logger = logger;
            this.companyService = companyService;
        }

        [HttpGet("all")]
        public override async Task<ActionResult<IEnumerable<Company>>> GetAll()
        {
            var result = await companyService.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("{id}")]
        public override async Task<ActionResult<Company>> GetById(int id)
        {
            var result = await companyService.FindById(id);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        public override async Task<ActionResult<Company>> GetByUUID(Guid UUID)
        {
            var result = await companyService.FindByUUID(UUID);

            return result != default ? Ok(result) : NotFound();

        }

        [HttpPost("add")]
        public override async Task<ActionResult<Company>> Add([FromBody] Company company)
        {
            var result = await companyService.Insert(company);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        public override async Task<ActionResult<Company>> Edit([FromBody] Company company)
        {
            var result = await companyService.Update(company);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        public override async Task<ActionResult<Company>> Remove(Company company)
        {
            var result = await companyService.Delete(company);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<ActionResult<Company>> RemoveById(int id)
        {
            var result = await companyService.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        public override async Task<ActionResult<Company>> RemoveByUUID(Guid UUID)
        {
            var result = await companyService.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
