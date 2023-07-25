using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.MedicineData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/medicine")]
    [ApiController]
    public class MedicineController : Controller<Medicine>
    {
        public MedicineController(ILogger<MedicineController> logger, IDbService<Medicine> service) : base(logger, service) { }

        [HttpGet("all")]
        public override async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
        {
            var result = await service.FindAll();

            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("{id:int}")]
        public override async Task<ActionResult<Medicine>> GetById(int id)
        {
            var result = await service.FindById(id);

            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        public override async Task<ActionResult<Medicine>> GetByUUID(Guid UUID)
        {
            var result = await service.FindByUUID(UUID);

            return result != default ? Ok(result) : NotFound();

        }

        [HttpPost("add")]
        public override async Task<ActionResult<Medicine>> Add([FromBody] Medicine medicine)
        {
            var result = await service.Insert(medicine);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        public override async Task<ActionResult<Medicine>> Edit([FromBody] Medicine medicine)
        {
            var result = await service.Update(medicine);

            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        public override async Task<ActionResult<Medicine>> Remove(Medicine medicine)
        {
            var result = await service.Delete(medicine);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<ActionResult<Medicine>> RemoveById(int id)
        {
            var result = await service.DeleteById(id);

            return result != 0 ? Ok() : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        public override async Task<ActionResult<Medicine>> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);

            return result != 0 ? Ok() : BadRequest();
        }
    }
}
