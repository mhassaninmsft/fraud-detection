using CreditCardSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CreditCardSimulator.Controllers
{
    [Route("odata")]
    [ApiController]
    public class CreditCardController : ODataController
    {
        private readonly MagicBankContext magicBankContext;
        public CreditCardController(MagicBankContext magicBankContext)
        {
            this.magicBankContext = magicBankContext;
        }
        [HttpGet("CreditCards")]
        [HttpGet("CreditCards/{id}")]

        [EnableQuery]
        public IActionResult GetCard(Guid? id)
        {
            if (id != null)
            {
                return Ok(magicBankContext.CreditCards.FirstOrDefault(c => c.Id == id));
            }
            else
            {
                return Ok(magicBankContext.CreditCards);
            }
        }

        [EnableQuery]
        [HttpPost("CreditCards")]
        public async Task<IActionResult> Post([FromBody] CreditCard card)
        {
            await magicBankContext.CreditCards.AddAsync(card);
            await magicBankContext.SaveChangesAsync();
            return Created(card);
        }
    }
}
