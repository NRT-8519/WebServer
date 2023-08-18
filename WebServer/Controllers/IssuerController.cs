﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models.MedicineData;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/issuer")]
    [ApiController]
    public class IssuerController : Controller<Issuer, Issuer, Issuer>
    {

        public IssuerController(ILogger<IssuerController> logger, IDbService<Issuer, Issuer, Issuer> service) : base(logger, service)
        {
        }

        [HttpGet("all")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await service.FindAll();
            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("{id:int}")]
        public override async Task<IActionResult> GetById(int id)
        {
            var result = await service.FindById(id);
            return result != default ? Ok(result) : NotFound();
        }

        [HttpGet("{UUID}")]
        public override async Task<IActionResult> GetByUUID(Guid UUID)
        {
            var result = await service.FindByUUID(UUID);
            return result != default ? Ok(result) : NotFound();
        }

        [HttpPost("add")]
        public override async Task<IActionResult> Add([FromBody] Issuer entity)
        {
            var result = await service.Insert(entity);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("edit")]
        public override async Task<IActionResult> Edit([FromBody] Issuer entity)
        {
            var result = await service.Update(entity);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove")]
        public override async Task<IActionResult> Remove([FromBody] Issuer entity)
        {
            var result = await service.Delete(entity);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove/{id}")]
        public override async Task<IActionResult> RemoveById(int id)
        {
            var result = await service.DeleteById(id);
            return result != 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("remove/{UUID}")]
        public override async Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            var result = await service.DeleteByUUID(UUID);
            return result != 0 ? Ok(result) : BadRequest();
        }
    }
}
