using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlastService.DataAccess.Controllers
{
    [Route("/")]
    [ApiController]
    public class DataController : Controller
    {
        private ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> Get()
        {
            _logger.LogInformation("Feed this hungry class :(");
            return Ok("Hello world!");
        }
    }
}
