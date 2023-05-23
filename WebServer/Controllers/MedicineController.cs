using Microsoft.AspNetCore.Mvc;
using WebServer.Models.MedicineData;

namespace WebServer.Controllers
{
    public class MedicineController : Controller<Medicine>
    {
        public override async Task<ActionResult<Medicine>> Add(Medicine entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<Medicine>> Edit(Medicine entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<Medicine>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<Medicine>> GetByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<Medicine>> Remove(Medicine entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<Medicine>> RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<Medicine>> RemoveByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
