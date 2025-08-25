using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;
using UrlShortener.Models;
using UrlShortener.ServiceContracts;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly IAnalyticsService _analyticsService;

        public DashboardController(IUrlService urlService, IAnalyticsService analyticsService)
        {
            _urlService = urlService;
            _analyticsService = analyticsService;
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
            return BadRequest();
        }

        [HttpGet]
        [Route("GetClicks{UrlId}")]
        [Authorize]
        public async Task<ActionResult> GetClickAnalytics(int UrlId)
        {
            int userId = int.Parse(User.Claims.First(c =>c.Type == ClaimTypes.NameIdentifier).Value);
            try
            {
                bool isUrlOwnedByUser = await _urlService.IsUrlOwnedByUser(userId, UrlId);
                if (isUrlOwnedByUser)
                {
                    ClicksResponseDTO clicks = await _analyticsService.GetClicksByUrlId(UrlId);

                    return Ok(clicks);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return BadRequest();
           
        }

        [HttpGet]
        [Route("Series")]
        [Authorize]
        public async Task<ActionResult> GetSeriesAnalytics([FromQuery] SeriesRequestDTO seriesRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage).ToList();
                return BadRequest(new { errors });
            }

            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            try
            {
                bool isUrlOwnedByUser = await _urlService.IsUrlOwnedByUser(userId, seriesRequest.UrlId);
                if (isUrlOwnedByUser)
                {
                    var series = await _analyticsService.GetLineByUrlId(seriesRequest);
                    return Ok(series);
                }
            }
            catch(Exception ex)
            {
                BadRequest(ex.Message);
            }
            return BadRequest();
            //throw new NotImplementedException();
        }
    }
}
