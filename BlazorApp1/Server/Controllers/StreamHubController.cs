using BlazorApp1.Shared;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamHubController : ControllerBase
    {
        private readonly IDbConnectionFactory connectionFactory;

        public StreamHubController(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
        [HttpPost("[action]")]
        public IEnumerable<string> Add(StreamerTask streamer)
        {
            return new string[] { "value1", "value2" };
        }
    
    }
}
