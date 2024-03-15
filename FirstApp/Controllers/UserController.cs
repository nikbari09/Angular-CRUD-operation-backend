using FirstApp.DataContect;
using FirstApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDataContext _myDataContext;
        private readonly IConfiguration _configuration;
        private SymmetricSecurityKey _key;

        public UserController(MyDataContext myDataContext, IConfiguration configuration)
        {
            _myDataContext = myDataContext;
            _configuration = configuration;
        }

        [HttpGet("getEmployee")]
        public ActionResult<List<Employees>> getEmp()
        {
            return _myDataContext.Employees.ToList();
        }

        [HttpPost("login")]
        public ActionResult<Employees> login(Employees employee)
        {
            var emp=_myDataContext.Employees.FirstOrDefault(x=>x.email == employee.email);
            if(emp == null)
            {
                return BadRequest("employee not found.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, employee.email),
                new Claim(JwtRegisteredClaimNames.UniqueName, employee.email),
            };

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = creds,
                Issuer = _configuration["ValidIssuer"],
                Audience = _configuration["ValidAudience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            var AuthModel = new
            {
                token = tokenHandler.WriteToken(token),
                valid = token.ValidTo
            };
            return Ok(AuthModel);
        }

        [HttpPost("adduser")]
        public ActionResult PostUser([FromBody] Employees userobj)
        {
            var isUserExist = _myDataContext.Employees.FirstOrDefault(x => x.email == userobj.email);

            if (isUserExist != null)
                return BadRequest("Email is already registered");

 

            _myDataContext.Employees.Add(userobj);
            _myDataContext.SaveChanges();

            return StatusCode((int)HttpStatusCode.Created, "User added successfully");
        }
    }
}
