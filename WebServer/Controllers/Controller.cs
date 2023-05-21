using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.Services;

namespace WebServer.Controllers
{
    public abstract class Controller<T> : ControllerBase
    {
        public Controller() { }

        public abstract Task<ActionResult<IEnumerable<T>>> GetAll(); 
        public abstract Task<ActionResult<T>> GetById(int id);
        public abstract Task<ActionResult<T>> GetByUUID(Guid UUID);
        public abstract Task<ActionResult<T>> Add(T entity);
        public abstract Task<ActionResult<T>> Edit(T entity);
        public abstract Task<ActionResult<T>> Remove(T entity);
        public abstract Task<ActionResult<T>> RemoveById(int id);
        public abstract Task<ActionResult<T>> RemoveByUUID(Guid UUID);
    }
}
