using Dinky.Infrastructure.Extensions;
using Dinky.Services.service;
using Dinky.WebApi.Extension;
using Dinky.WebApi.ViewModels;
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
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        Lazy<IUserRepository> _userRepository;

        public AuthController(Lazy<IUserRepository> UserRepository)
        {
            _userRepository = UserRepository;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var user = await _userRepository.Value.SELECTAsync(model.username);

            if (user != null && IdentityCryptography.VerifyHashedPasswordOld(user.pass, model.password))
            {
                if (user.enab == 0)
                    return Unauthorized();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {

                        new Claim(ClaimTypes.MobilePhone, user.mobile),
                         new Claim(ClaimTypes.Name, user.code.ToString())
                    };

                var tokeOptions = new JwtSecurityToken(
                    null, null,
                    claims: claims,
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new
                {
                    Token = tokenString,
                    TokenExpirationTime = ((DateTimeOffset)DateTime.Now.AddHours(12)).ToUnixTimeSeconds(),
                    Id = user.code
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        //[HttpPost, Route("forgetPassword")]
        //public IActionResult forgetPassword([FromBody]ForgetPasswordModel model)
        //{
        //    if (model == null)
        //    {
        //        return BadRequest("Invalid client request");
        //    }

        //    var user = _userRepository.Value.Read(model.username);

        //    _smsRepository.Value.Send($"کد فعال سازی : {infoVerify.verifyCode}");

        //    return Ok(new { userId = user.userId });
        //}

        //[HttpPost, Route("ChangePassword")]
        //public IActionResult ChangePassword([FromBody]ChangePasswordModel model)
        //{
        //    if (model == null)
        //    {
        //        return BadRequest("Invalid client request");
        //    }
        //    var verify = _infoVerifyRepository.Value.VerifyMobile(model.username, model.verifyCode);

        //    if (verify)
        //    {
        //        var user = _userRepository.Value.Read(model.username).FirstOrDefault();

        //        user.password = IdentityCryptography.HashPassword(model.password);

        //        _smsRepository.Value.Send($"رمز شما تغییر کرد");
        //        _uow.Save();
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest("Invalid VerifyCode");
        //    }
        //}

    }
}