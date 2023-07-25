using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Authentication;
using WebServer.Models;
using WebServer.Models.UserData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller<User>
    {
        private readonly ITokenService<JWTToken> tokenService;

        public UserController(ILogger<UserController> logger, IDbService<User> service, ITokenService<JWTToken> tokenService) : base(logger, service)
        {
            this.tokenService = tokenService;
        }

        [HttpGet("all")]
        public override async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var result = await service.FindAll();
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

        [HttpGet("{id:int}")]
        public override async Task<ActionResult<User>> GetById(int id)
        {
            var user = await service.FindById(id);
            if (user != default) 
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{UUID}")]
        public override async Task<ActionResult<User>> GetByUUID(Guid UUID)
        {
            var user = await service.FindByUUID(UUID);
            if (user != default)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        public override async Task<ActionResult<User>> Add([FromBody] User model)
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
        public override async Task<ActionResult<User>> Edit([FromBody] User model)
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
        public override async Task<ActionResult<User>> Remove([FromBody] User model)
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
        public override async Task<ActionResult<User>> RemoveById(int id)
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
        public override async Task<ActionResult<User>> RemoveByUUID(Guid UUID)
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
            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(token);
            }
        }
    }
}
