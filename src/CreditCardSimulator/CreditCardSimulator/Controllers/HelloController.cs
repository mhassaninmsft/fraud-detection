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
        [HttpGet]
        [Route("customfunc/{num1}/{num2}")]
        public async Task<string> customFunc(int num1, int num2)
        {
            var lists1 = await magicBankContext1.PosMachines.Select(s => magicBankContext1.ConcatTwoMethod(s.ZipCode, "hello")).ToListAsync();
            lists1.ForEach(elem => Console.WriteLine(elem));
            //var lists = magicBankContext1.ConcatTwoMethod(num1.ToString(), num2.ToString());
            var res1 = await magicBankContext1.SearchByYear(23).ToListAsync();
            res1.ForEach(cr => Console.WriteLine($"{cr.Id},{cr.ExpirationYear}"));
            return lists1.ToString();
        }
        [HttpGet]
        [Route("customfunc2/{num1}/{num2}")]
        public async Task<string> customFunc2(int num1, int num2)
        {
            //var lists1 = await magicBankContext1.PosMachines.Select(s => magicBankContext1.ConcatTwoMethod(s.ZipCode, "hello")).ToListAsync();
            //lists1.ForEach(elem => Console.WriteLine(elem));
            //var lists = magicBankContext1.ConcatTwoMethod(num1.ToString(), num2.ToString());
            //IQueryable<int> res = magicBankContext1.FromExpression(() => magicBankContext1.AddTwoMethod(num1, num2));
            //var res1 = await magicBankContext1.SearchByYear(23).ToListAsync();
            //res1.ForEach(cr => Console.WriteLine($"{cr.Id},{cr.ExpirationYear}"));
            var res = await magicBankContext1.AddTwoNumbers(num1, num2);
            return res.ToString();
        }
    }
}
