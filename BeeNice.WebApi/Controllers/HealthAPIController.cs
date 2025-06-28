using BeeNice.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BeeNice.WebApi.Controllers
{
    [Route("api/HealthApiController")]
    [ApiController]
    public class HealthAPIController : BaseController
    {
        [HttpGet("HealthCheck")]
        public ActionResult HealthCheck()
        {
            var requestIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // If we use Nginx, we get the real IP from the X-Forwarded-For header
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                requestIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            // Check taht IP is coming from localhost
            if (requestIp != "127.0.0.1" && requestIp != "::1")
            {
                return Unauthorized();
            }

            return Ok();
        }
    }
}
