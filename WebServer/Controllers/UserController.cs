using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Authentication;
using WebServer.Models;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IMariaDbService<UserModel> mariaDbService;
        private readonly ITokenService<JWTToken> tokenService;

        public UserController(ILogger<UserController> logger, IMariaDbService<UserModel> mariaDbService, ITokenService<JWTToken> tokenService)
        {
            this.logger = logger;
            this.mariaDbService = mariaDbService;
            this.tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
        {
            var result = await mariaDbService.FindAll();
            if (result.Any()) 
            {
                return Ok(result);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> Get(int id)
        {
            var user = await mariaDbService.FindById(id);
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
        public async Task<ActionResult<UserModel>> Add([FromBody] UserModel model)
        {
            var user = await mariaDbService.Insert(model);

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
        public async Task<ActionResult<UserModel>> Edit([FromBody] UserModel model)
        {
            var user = await mariaDbService.Update(model);

            if (user != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("removeById/{id}")]
        public async Task<ActionResult<UserModel>> RemoveById(int id)
        {
            var user = await mariaDbService.DeleteById(id);

            if (user != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("removeByUUID/{UUID}")]
        public async Task<ActionResult<UserModel>> RemoveByUUID(Guid UUID)
        {
            var user = await mariaDbService.DeleteByUUID(UUID);

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
        public IActionResult Authenticate(LoginModel user)
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
