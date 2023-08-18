using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.Services;

namespace WebServer.Controllers
{
    public abstract class Controller<T, B, D> : ControllerBase
    {
        protected readonly ILogger logger;
        protected readonly IDbService<T, B, D> service;

        public Controller(ILogger logger, IDbService<T, B, D> service) 
        {
            this.logger = logger;
            this.service = service;
        }

        public abstract Task<IActionResult> GetAll(); 
        public abstract Task<IActionResult> GetById(int id);
        public abstract Task<IActionResult> GetByUUID(Guid UUID);
        public abstract Task<IActionResult> Add(T entity);
        public abstract Task<IActionResult> Edit(D entity);
        public abstract Task<IActionResult> Remove(D entity);
        public abstract Task<IActionResult> RemoveById(int id);
        public abstract Task<IActionResult> RemoveByUUID(Guid UUID);
    }
}
