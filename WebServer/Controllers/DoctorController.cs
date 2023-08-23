using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using WebServer.Authentication;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Models.UserData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/users/doctors")]
    [ApiController]
    public class DoctorController : Controller<Doctor, UserBasicDTO, DoctorDetailsDTO>
    {
        public DoctorController(ILogger<DoctorController> logger, IDbService<Doctor, UserBasicDTO, DoctorDetailsDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all/basic")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR")]
        public async Task<IActionResult> GetAllBasic(string sortOrder, string searchString, string currentFilter, int? pageNumber, int pageSize)
        {
            var result = await ((DoctorService)service).FindAllPaged(sortOrder, searchString, currentFilter, pageNumber, pageSize);
            if (result.Any())
            {
                PaginatedResultDTO<UserBasicDTO> DTOs = new();
                DTOs.PageNumber = result.PageIndex;
                DTOs.PageSize = result.PageSize;
                DTOs.TotalPages = result.TotalPages;
                DTOs.TotalItems = result.TotalItems;
                DTOs.HasNext = result.HasNextPage;
                DTOs.HasPrevious = result.HasPreviousPage;
                foreach (var doctor in result)
                {
                    DTOs.items.Add(new UserBasicDTO
                    {
                        FirstName = doctor.FirstName,
                        MiddleName = doctor.MiddleName,
                        LastName = doctor.LastName,
                        Username = doctor.Username,
                        Email = doctor.Email,
                        UUID = doctor.UUID
                    });
                }
                logger.LogInformation("Fetched all doctors basic information (Paged).");
                return Ok(DTOs);
            }
            else
            {
                logger.LogInformation("Doctors database empty.");
                return NoContent();
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await ((DoctorService)service).FindAll();

            if (result.Any())
            {
                List<UserBasicDTO> DTOs = new();
                foreach (var doctor in result)
                {
                    DTOs.Add(new UserBasicDTO
                    {
                        FirstName = doctor.FirstName,
                        MiddleName = doctor.MiddleName,
                        LastName = doctor.LastName,
                        Username = doctor.Username,
                        Email = doctor.Email,
                        UUID = doctor.UUID
                    });
                }
                logger.LogInformation("Fetched all doctors.");
                return Ok(DTOs);
            }
            else
            {
                logger.LogInformation("Doctors database empty.");
                return NoContent();
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetById(int id)
        {
            var doctor = await ((DoctorService)service).FindById(id);
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
            var result = await ((DoctorService)service).FindByUUID(UUID);
            if (result != default)
            {
                DoctorDetailsDTO doctor = new()
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
                    AreaOfExpertise = result.AreaOfExpertise,
                    RoomNumber = result.RoomNumber
                };

                return Ok(doctor);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Add([FromBody] DoctorDetailsDTO model)
        {
            var user = await ((DoctorService)service).Insert(model);

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
        public override async Task<IActionResult> Edit([FromBody] DoctorDetailsDTO model)
        {
            var user = await ((DoctorService)service).Update(model);

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
