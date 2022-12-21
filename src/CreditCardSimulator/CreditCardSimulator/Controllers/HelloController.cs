using Microsoft.AspNetCore.Mvc;

namespace CreditCardSimulator.Controllers
{
    [Route("api/hello")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        [Route("basic")]
        public string SayHello()
        {
            return "hello there";
        }
    }
}
