using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        [HttpGet]
        [Route("myLinks")]
        [Authorize]
        public ActionResult GetMyLinks()
        {
            return Ok();
        }
    }
}
