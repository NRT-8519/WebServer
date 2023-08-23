using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/users/patients")]
    [ApiController]
    public class PatientController : Controller<Patient, UserBasicDTO, PatientDetailsDTO>
    {
        public PatientController(ILogger<PatientController> logger, IDbService<Patient, UserBasicDTO, PatientDetailsDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await ((PatientService)service).FindAll();
            if (result.Any())
            {
                List<UserBasicDTO> DTOs = new();
                foreach (var patient in result)
                {
                    DTOs.Add(new UserBasicDTO
                    {
                        UUID = patient.UUID,
                        FirstName = patient.FirstName,
                        MiddleName = patient.MiddleName,
                        LastName = patient.LastName,
                        Username = patient.Username,
                        Email = patient.Email
                    });
                }
                logger.LogInformation("Fetched all patients basic information.");
                return Ok(DTOs);
            }
            else
            {
                logger.LogInformation("Patients database empty.");
                return NoContent();
            }
        }

        [HttpGet("all/basic")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR")]
        public async Task<IActionResult> GetAllBasic(string sortOrder, string searchString, string currentFilter, int? pageNumber, int pageSize)
        {
            var result = await ((PatientService)service).FindAllPaged(sortOrder, searchString, currentFilter, pageNumber, pageSize);
            if (result.Any())
            {
                PaginatedResultDTO<UserBasicDTO> DTOs = new();
                DTOs.PageNumber = result.PageIndex;
                DTOs.PageSize = result.PageSize;
                DTOs.TotalPages = result.TotalPages;
                DTOs.TotalItems = result.TotalItems;
                DTOs.HasNext = result.HasNextPage;
                DTOs.HasPrevious = result.HasPreviousPage;
                foreach (var patient in result)
                {
                    DTOs.items.Add(new UserBasicDTO
                    {
                        FirstName = patient.FirstName,
                        MiddleName = patient.MiddleName,
                        LastName = patient.LastName,
                        Username = patient.Username,
                        Email = patient.Email,
                        UUID = patient.UUID
                    });
                }
                logger.LogInformation("Fetched all patients basic information (Paged).");
                return Ok(DTOs);
            }
            else
            {
                logger.LogInformation("Patients database empty.");
                return NoContent();
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetById(int id)
        {
            var doctor = await ((PatientService)service).FindById(id);
            if (doctor != default)
            {
                return Ok(doctor);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{UUID}")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR,PATIENT")]
        public override async Task<IActionResult> GetByUUID(Guid UUID)
        {
            var result = await ((PatientService)service).FindByUUID(UUID);
            
            if (result != default)
            {
                PatientDetailsDTO patient = new()
                {
                    UUID = result.UUID,
                    FirstName = result.FirstName,
                    MiddleName = result.MiddleName,
                    LastName = result.LastName,
                    Username = result.Username,
                    Title = result.Title,
                    DateOfBirth = result.DateOfBirth,
                    Gender = result.Gender,
                    SSN = result.SSN,
                    Email = result.Email,
                    PhoneNumber = result.PhoneNumber,
                    PasswordExpiryDate = result.PasswordExpiryDate,
                    IsDisabled = result.IsDisabled,
                    IsExpired = result.IsExpired,
                    AssignedDoctor = new UserBasicDTO
                    {
                        FirstName = result.AssignedDoctor.FirstName,
                        MiddleName = result.AssignedDoctor.MiddleName,
                        LastName = result.AssignedDoctor.LastName,
                        Username = result.AssignedDoctor.Username,
                        Email = result.AssignedDoctor.Email,
                        UUID = result.AssignedDoctor.UUID
                    }
                };
                return Ok(patient);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Add([FromBody] PatientDetailsDTO model)
        {
            var user = await ((PatientService)service).Insert(model);

            if (user != 0)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("edit")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR")]
        public override async Task<IActionResult> Edit([FromBody] PatientDetailsDTO model)
        {
            var user = await ((PatientService)service).Update(model);

            if (user != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("remove")]
        [Obsolete]
        public override async Task<IActionResult> Remove(UserBasicDTO entity)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{id}")]
        [Obsolete]
        public override async Task<IActionResult> RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{UUID}")]
        [Obsolete]
        public override async Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
