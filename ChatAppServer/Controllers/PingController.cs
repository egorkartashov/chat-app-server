using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public Task<ActionResult> Get()
        {
            var response = new { message = "GET pong!!!" };
            return Task.FromResult<ActionResult>(Ok(response));
        }

        [HttpPost]
        public Task<ActionResult> Post([FromBody] object data)
        {
            var response = new { message = "POST pong!!!" };
            return Task.FromResult<ActionResult>(Ok(response));
        }
    }
}