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
    public class UserController : Controller<User, User, User>
    {
        private readonly ITokenService<JWTToken> tokenService;

        public UserController(ILogger<UserController> logger, IDbService<User, User, User> service, ITokenService<JWTToken> tokenService) : base(logger, service)
        {
            this.tokenService = tokenService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();
            List<UserDTO> DTOs = new List<UserDTO>();
            foreach (User user in result)
            {
                DTOs.Add(new UserDTO { 
                    UUID = user.UUID, 
                    UserName = user.Username, 
                    FirstName = user.FirstName, 
                    MiddleName = user.MiddleName, 
                    LastName = user.LastName, 
                    Title = user.Title, 
                    DateOfBirth = user.DateOfBirth, 
                    SSN = user.SSN, 
                    Gender = user.Gender, 
                    Email = user.Email, 
                    PhoneNumber = user.PhoneNumber, 
                    Role = user.Role, 
                    IsExpired = user.IsExpired, 
                    IsDisabled = user.IsDisabled, 
                    PasswordExpiry = user.PasswordExpiryDate
                });
            }
            if (result.Any()) 
            {
                logger.LogInformation("Fetched all users.");
                return Ok(DTOs);
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

                return Ok(new UserDTO
                {
                    UUID = user.UUID,
                    UserName = user.Username,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Title = user.Title,
                    DateOfBirth = user.DateOfBirth,
                    SSN = user.SSN,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    IsExpired = user.IsExpired,
                    IsDisabled = user.IsDisabled,
                    PasswordExpiry = user.PasswordExpiryDate
                });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{UUID}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetByUUID(Guid UUID)
        {
            var user = await service.FindByUUID(UUID);
            if (user != default)
            {
                return Ok(new UserDTO
                {
                    UUID = user.UUID,
                    UserName = user.Username,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Title = user.Title,
                    DateOfBirth = user.DateOfBirth,
                    SSN = user.SSN,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    IsExpired = user.IsExpired,
                    IsDisabled = user.IsDisabled,
                    PasswordExpiry = user.PasswordExpiryDate
                });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Add([FromBody] User model)
        {
            var user = await service.Insert(model);

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
        public override async Task<IActionResult> Edit([FromBody] User model)
        {
            var user = await service.Update(model);

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
        public override async Task<IActionResult> Remove([FromBody] User model)
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
