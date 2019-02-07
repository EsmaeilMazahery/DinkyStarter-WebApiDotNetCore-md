using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Dinky.WebApi.Controllers
{
    [AllowAnonymous]
    public class BaseApiController : Controller
    {
        public BaseApiController()
        {
        }

        public int? userId
        {
            get
            {
                string userId = User?.Identity.Name;
                return userId != null ? int.Parse(userId) : (int?)null;
            }
        }
    }
}
