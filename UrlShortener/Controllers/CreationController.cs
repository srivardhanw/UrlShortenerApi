using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Security.Claims;
using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;
using UrlShortener.ServiceContracts;
using UrlShortener.Utilities;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CreationController : ControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly IGeolocationService _geolocationService;
        public CreationController(IUrlService urlService, IGeolocationService geolocationService) {
            _urlService = urlService;
            _geolocationService = geolocationService;
        }
        [HttpPost]
        [Route("shortenUrl")]
        [Authorize]
        public async Task<ActionResult> Shorten([FromBody] OriginalRequestDTO req)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage).ToList();
                return BadRequest(new { errors });
            }

            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            
            try
            {
                var res = await _urlService.GetShortUrl(userId, req);
                return Ok(res);
            }
            catch(Exception err)
            {
                return BadRequest(err);
            }
        }

        [HttpGet("RedirectToOriginal/{ShortId}")]
        public async Task<IActionResult> RedirectToOriginal(string ShortId)
        {
            string userAgent = Request.Headers["User-Agent"].ToString();
            string deviceType = UserAgentHelper.GetDeviceName(userAgent) ?? "unknown";
            string? ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ip == "::1") ip = "122.175.10.148";
            }
           
            try
            {
                var res = await _urlService.GetLongUrl(ShortId, deviceType, ip);
                return Ok(res);
            }
            catch(SqlException err)
            {
                return BadRequest(err);
            }
        }

        [HttpGet]
        [Route("GetGeolocation/{IpAddress}")]
        public async Task<ActionResult> GeolocationTest(string IpAddress)
        {
            var res = await _geolocationService.GetGeolocationAsync(IpAddress);
            return Ok( res);
        }

       

        [HttpDelete]
        [Route("DeleteUrl")]
        [Authorize]
        public async Task<ActionResult> DeleteUrl([FromQuery] int UrlId)
        {
            return Ok();
        }



    }
}
