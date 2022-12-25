using CreditCardSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CreditCardSimulator.Controllers
{
    [Route("odata")]
    [ApiController]
    public class PosMachineController : ODataController
    {
        private readonly MagicBankContext magicBankContext;
        public PosMachineController(MagicBankContext magicBankContext)
        {
            this.magicBankContext = magicBankContext;
        }
        [HttpGet("PosMachines")]
        [HttpGet("PosMachines/{id}")]

        [EnableQuery]
        public IActionResult GetPosMachine(Guid? id)
        {
            if (id != null)
            {
                return Ok(magicBankContext.PosMachines.FirstOrDefault(c => c.Id == id));
            }
            else
            {
                return Ok(magicBankContext.PosMachines);
            }
        }

        [EnableQuery]
        [HttpPost("PosMachines")]
        public async Task<IActionResult> Post([FromBody] PosMachine machine)
        {
            await magicBankContext.PosMachines.AddAsync(machine);
            await magicBankContext.SaveChangesAsync();
            return Created(machine);
        }
    }
}
