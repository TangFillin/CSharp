using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JWTTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpContextAccessor _accessor;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            //创建一个简单的token令牌

            //创建声明数组
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "fillin"),
                new Claim(ClaimTypes.Email,"1035@qq.com"),
                new Claim(JwtRegisteredClaimNames.Email,"1035@qq.com"),
                new Claim(JwtRegisteredClaimNames.Sub, "123")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fillin123456ttffll"));//密钥,>=位16
            //实例化token对象
            var token = new JwtSecurityToken(
                issuer:"http://localhost:5000",//发行人
                audience:"http://localhost:5000",//
                claims: claims,
                expires:DateTime.Now.AddHours(1),//过期日期
                signingCredentials:new SigningCredentials(key,SecurityAlgorithms.HmacSha256)//数字签名
                );
            
            //生成token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new string[] { jwtToken };
        }
        [HttpGet("{jwtstr}")]
        public ActionResult<IEnumerable<string>> Get1()
        {
            //获取token

            //1
            //var jwtHandler = new JwtSecurityTokenHandler();
            //JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            //2
            var sub = User.FindFirst(d => d.Type == JwtRegisteredClaimNames.Sub)?.Value;

            //3
            var name = _accessor.HttpContext.User.Identity.Name;
            var claims = _accessor.HttpContext.User.Claims;

            var claimTypeValue = (from item in claims
                                  where item.Type == JwtRegisteredClaimNames.Email
                                  select item.Value).ToList();
            return new string[] { sub, name,JsonConvert.SerializeObject(claimTypeValue)};
        }
    }
}
