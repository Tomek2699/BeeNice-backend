using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeeNice.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        internal string? GetUserId()
        {
            return HttpContext.Items["UserId"] as string;
        }
    }
}
