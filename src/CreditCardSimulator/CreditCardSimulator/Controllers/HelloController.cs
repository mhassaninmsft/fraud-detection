using CreditCardSimulator.Database;
using CreditCardSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditCardSimulator.Controllers
{
    [Route("api/hello")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly MagicBankContext magicBankContext1;
        private readonly Functions functions;
        public HelloController(MagicBankContext magicBankContext, Functions functions)
        {
            magicBankContext1 = magicBankContext;
            this.functions = functions;
        }
        [HttpGet]
        [Route("basic")]
        public async Task<string> SayHello()
        {
            var lists = await magicBankContext1.PosMachines.Select(s => s).ToListAsync();
            lists.ForEach(elem => Console.WriteLine($"{elem.ZipCode}, {elem.Id}"));
            await functions.AddTwo(2, 3);
            return "hello there";
        }
    }
}
