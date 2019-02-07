using Dinky.Infrastructure.Extensions;
using Dinky.Services.service;
using Dinky.WebApi.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dinky.WebApi.Controllers
{
    [Route("api/baseinfo")]
    [Produces("application/json")]
    public class BaseinfoController : ControllerBase
    {
        [HttpGet,Route("companyInfo")]
        public async Task<IActionResult> Get()
        {
            return Ok(new {companyName="ماهان نت"});
        }
    }
}