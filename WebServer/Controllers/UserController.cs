using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Authentication;
using WebServer.Models;
using WebServer.Models.DTOs;
using WebServer.Models.UserData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller<User, UserBasicDTO, UserDetailsDTO>
    {
        private readonly ITokenService<JWTToken> tokenService;

        public UserController(ILogger<UserController> logger, IDbService<User, UserBasicDTO, UserDetailsDTO> service, ITokenService<JWTToken> tokenService) : base(logger, service)
        {
            this.tokenService = tokenService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await ((UserService)service).FindAll();
            if (result.Any()) 
            {
                logger.LogInformation("Fetched all users.");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("User database empty.");
                return NoContent();
            }
        }

        [HttpGet("administrators/count")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> GetAllAdministratorsCount()
        {
             return Ok(await ((UserService)service).FindAllAdministratorsCount()); 
        }

        [HttpGet("doctors/count")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> GetAllDoctorsCount()
        {
            return Ok(await ((UserService)service).FindAllDoctorsCount());
        }

        [HttpGet("patients/count")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> GetAllPatientsCount()
        {
            return Ok(await ((UserService)service).FindAllPatientsCount());
        }

        [HttpGet("all/basic")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> GetAllPaged(string sortOrder, string searchQuery, string currentFilter, int? pageNumber, int pageSize)
        {
            var result = await ((UserService)service).FindAllAdministratorsPaged(sortOrder, searchQuery, currentFilter, pageNumber, pageSize);
            if (result.items.Any())
            {
                logger.LogInformation("Fetched all users.");
                return Ok(result);
            }
            else
            {
                logger.LogInformation("User database empty.");
                return NoContent();
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetById(int id)
        {
            var user = await service.FindById(id);
            if (user != default) 
            {

                return Ok(new UserDetailsDTO
                {
                    UUID = user.UUID,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Title = user.Title,
                    DateOfBirth = user.DateOfBirth,
                    SSN = user.SSN,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsExpired = user.IsExpired,
                    IsDisabled = user.IsDisabled,
                    PasswordExpiryDate = user.PasswordExpiryDate
                });
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
            var user = await ((UserService)service).FindByUUID(UUID);
            if (user != default)
            {
                return Ok(new UserDetailsDTO
                {
                    UUID = user.UUID,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Title = user.Title,
                    DateOfBirth = user.DateOfBirth,
                    SSN = user.SSN,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsExpired = user.IsExpired,
                    IsDisabled = user.IsDisabled,
                    PasswordExpiryDate = user.PasswordExpiryDate
                });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Add([FromBody] UserDetailsDTO model)
        {
            var user = await ((UserService)service).InsertAdministrator(model);

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
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Edit([FromBody] UserDetailsDTO model)
        {
            var user = await ((UserService)service).UpdateAdministrator(model);

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
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Remove([FromBody] UserBasicDTO model)
        {
            var user = await service.Delete(model);

            if (user != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> RemoveById(int id)
        {
            var user = await service.DeleteById(id);

            if (user != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("remove/{UUID}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            var user = await service.DeleteByUUID(UUID);

            if (user != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(Login user)
        {
            var token = tokenService.Authenticate(user);
            if (token == null || token.IsAuthSuccessful == false)
            {
                return Unauthorized(token);
            }
            else
            {
                return Ok(token);
            }
        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public IActionResult Validate(string token)
        {
            return tokenService.Validate(token) ? Ok(true) : Unauthorized(false);
        }
    }
}
