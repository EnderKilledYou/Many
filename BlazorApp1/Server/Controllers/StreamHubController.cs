using BlazorApp1.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamHubController : ControllerBase
    {
        [HttpPost("[action]")]
        public IEnumerable<string> Add(StreamerTask streamer)
        {
            return new string[] { "value1", "value2" };
        }
    
    }
}
