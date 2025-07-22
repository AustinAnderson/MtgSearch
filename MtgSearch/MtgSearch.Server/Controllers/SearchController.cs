using Microsoft.AspNetCore.Mvc;

namespace MtgSearch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;

        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpPost("CheckSearchCount")]
        public IActionResult CountSearch()
        {
        }
        [HttpPost("RunSearch")]
        public IActionResult DoSearch()
        {
        }
    }
}
