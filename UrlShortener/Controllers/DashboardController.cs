using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.ServiceContracts;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public DashboardController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet]
        [Route("GetMyUrls")]
        [Authorize]
        public async Task<ActionResult> GetAllMyUrls()
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            try
            {
                var res = await _urlService.GetUrlsByUserId(userId);
                return Ok(res);
            }
            catch(Exception ex)
            {
                BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
