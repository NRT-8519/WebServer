using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.MedicineData;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/medicine")]
    [ApiController]
    public class MedicineController : Controller<Medicine>
    {

        [HttpPost("add")]
        public override async Task<ActionResult<Medicine>> Add([FromBody] Medicine entity)
        {
            throw new NotImplementedException();
        }

        [HttpPut("edit")]
        public override async Task<ActionResult<Medicine>> Edit([FromBody] Medicine entity)
        {
            throw new NotImplementedException();
        }

        [HttpGet("all")]
        public override async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("{id}")]
        public override async Task<ActionResult<Medicine>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{UUID}")]
        public override async Task<ActionResult<Medicine>> GetByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("remove")]
        public override async Task<ActionResult<Medicine>> Remove(Medicine entity)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<ActionResult<Medicine>> RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{UUID}")]
        public override async Task<ActionResult<Medicine>> RemoveByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
