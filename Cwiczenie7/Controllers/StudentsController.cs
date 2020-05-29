using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cwiczenie7.DTO_s;
using Cwiczenie7.DTOs.Requests;
using Cwiczenie7.Encryption;
using Cwiczenie7.Models;
using Cwiczenie7.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cwiczenie7.Controllers
{
    [ApiController]
    [Route("api/students")]
    //[Authorize(Roles = "employee")]
    public class StudentsController : ControllerBase
    {
        private IStudentDbService _dbService;
        public IConfiguration Configuration { get; set; }

        public StudentsController(IStudentDbService service, IConfiguration configuraton)
        {
            _dbService = service;
            Configuration = configuraton;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetStudents()
        {
            /* var dane = _dbService.GetStudents();
              var list = new List<Student>();
              var st = new Student();
              list.Add(st.IndexNumber);
              list.Add(st.FirstName);
              list.Add(st.LastName);
              list.Add(st.BirthDate);
              list.Add(st.StudiesName);
              st.FirstName = dane.FirstName;
              st.LastName = dane.LastName;
              st.BirthDate = Convert.ToDateTime(request.BirthDate);
              st.StudiesName = request.StudiesName;

              _dbService.GetStudents();
              var response = new EnrollStudentResponse();
              response.IndexNumber = st.IndexNumber;
              response.FirstName = st.FirstName;
              response.LastName = st.LastName;
              response.BirthDate = Convert.ToDateTime(st.BirthDate);
              response.StudiesName = st.StudiesName;
              list.Add(response);
              //return Ok(_dbService.EnrollStudent(request));
              return Ok(list);*/
            return Ok(_dbService.GetStudents());


        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {

            return Ok(_dbService.GetStudent(id));
        }
        
        [HttpPost]
        public IActionResult Login(LoginRequestDto request)
        {
            // sprawdzanie hasla w db
            string pass = request.Passw;
            string index = request.IndexNumber;

            if(pass == null && index == null)
            {
                throw new Exception("Index number and password cannot be null.");
            }

            if (index == User.Identity.Name)
            {

            }
            var claims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, index),
                new Claim(ClaimTypes.Name, index),
                new Claim(ClaimTypes.Role, "employee")

            };
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(pass));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var keystring = key.ToString();
            var salt = Encrypt.CreateSalt();
            var encrypted = Encrypt.Create(keystring, salt);

            var token = new JwtSecurityToken(

                issuer: "SandCorp",
                audience: "Employees",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials

                );
            return Ok(new
            {

                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()

            });
        }

        [HttpPost("refresh-token/{token}")]
        public IActionResult RefreshToken(string refreshToken)
        {
            return Ok();
        }

    }
}
