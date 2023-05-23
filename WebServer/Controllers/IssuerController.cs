using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using WebServer.Models.MedicineData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/issuers")]
    [ApiController]
    public class IssuerController : Controller<Issuer>
    {
        private readonly ILogger<IssuerController> logger;
        private readonly IDbService<Issuer> issuerService;

        public IssuerController(ILogger<IssuerController> logger, IDbService<Issuer> issuerService)
        {
            this.logger = logger;
            this.issuerService = issuerService;
        }

        [HttpGet("all")]
        public override async Task<ActionResult<IEnumerable<Issuer>>> GetAll()
        {
            var result = await issuerService.FindAll();
            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("{id}")]
        public override async Task<ActionResult<Issuer>> GetById(int id)
        {
            var result = await issuerService.FindById(id);
            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        public override async Task<ActionResult<Issuer>> GetByUUID(Guid UUID)
        {
            var result = await issuerService.FindByUUID(UUID);
            return result != default ? Ok(result) : NotFound();
        }

        [HttpPost("add")]
        public override async Task<ActionResult<Issuer>> Add([FromBody] Issuer entity)
        {
            var result = await issuerService.Insert(entity);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        public override async Task<ActionResult<Issuer>> Edit([FromBody] Issuer entity)
        {
            var result = await issuerService.Update(entity);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        public override async Task<ActionResult<Issuer>> Remove([FromBody] Issuer entity)
        {
            var result = await issuerService.Delete(entity);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<ActionResult<Issuer>> RemoveById(int id)
        {
            var result = await issuerService.DeleteById(id);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        public override async Task<ActionResult<Issuer>> RemoveByUUID(Guid UUID)
        {
            var result = await issuerService.DeleteByUUID(UUID);
            return result != 0 ? Ok(result) : BadRequest();
        }
    }
}
