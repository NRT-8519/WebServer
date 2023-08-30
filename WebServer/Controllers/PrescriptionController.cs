using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.ClinicData;
using WebServer.Models.DTOs;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionController : Controller<Prescription, PrescriptionDTO, PrescriptionDTO>
    {

        public PrescriptionController(ILogger<PrescriptionController> logger, IDbService<Prescription, PrescriptionDTO, PrescriptionDTO> service) : base(logger, service)
        {
        }

        [HttpPost("add")]
        [Authorize(Roles = "DOCTOR")]
        public async override Task<IActionResult> Add([FromBody] PrescriptionDTO entity)
        {
            var result = await((PrescriptionService)service).Insert(entity);
            if (result != 0)
            {
                logger.LogInformation("Fetched all prescriptions basic information.");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Prescriptions database empty.");
                return NoContent();
            }
        }

        [HttpPut("edit")]
        [Obsolete]
        public override Task<IActionResult> Edit([FromBody] PrescriptionDTO entity)
        {
            throw new NotImplementedException();
        }

        [HttpGet("all/all")]
        [Authorize(Roles = "DOCTOR")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await((PrescriptionService)service).FindAll();
            if (result.Any())
            {
                logger.LogInformation("Fetched all prescriptions basic information.");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Prescriptions database empty.");
                return NoContent();
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public async Task<IActionResult> GetAllPaged([FromQuery] string sortOrder, [FromQuery] int? pageNumber, [FromQuery] int pageSize, [FromQuery] Guid? doctor, [FromQuery] Guid? patient)
        {
            var result = await ((PrescriptionService)service).FindAllPaged(sortOrder, pageNumber, pageSize, doctor, patient);
            if (result.items.Any())
            {
                logger.LogInformation("Fetched all prescriptions basic information (Paged).");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Prescriptions database empty.");
                return NoContent();
            }
        }

        [HttpGet("count")]
        [Authorize(Roles = "PATIENT,DOCTOR,ADMINISTRATOR")]
        public async Task<IActionResult> GetCount(Guid UUID)
        {
            var result = await ((PrescriptionService)service).Count(UUID);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "PATIENT,DOCTOR")]
        public async override Task<IActionResult> GetById(int id)
        {
            var result = await((PrescriptionService)service).FindByIdAsDTO(id);
            if (result != null)
            {
                logger.LogInformation("Fetched all prescriptions basic information.");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Prescriptions database empty.");
                return NoContent();
            }
        }

        [HttpGet("{UUID}")]
        [Obsolete]
        public override Task<IActionResult> GetByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove")]
        [Obsolete]
        public override Task<IActionResult> Remove([FromBody] PrescriptionDTO entity)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{id:int}")]
        [Obsolete]
        public override Task<IActionResult> RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{UUID}")]
        [Obsolete]
        public override Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
