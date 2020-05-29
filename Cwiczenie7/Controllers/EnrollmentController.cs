using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cwiczenie7.Services;
using Cwiczenie7.DTOs.Requests;
using Cwiczenie7.Models;
using Cwiczenie7.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Cwiczenie7.DTO_s;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Cwiczenie7.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    [Authorize(Roles = "employee")]
    public class EnrollmentsController : ControllerBase
    {

        private IStudentDbService _dbService;
        public IConfiguration Configuration { get; set; }

        public EnrollmentsController(IStudentDbService service, IConfiguration configuraton)
        {
            _dbService = service;
            Configuration = configuraton;
        }
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {

            var list = new List<EnrollStudentResponse>();
            var st = new Student();
            st.IndexNumber = request.IndexNumber;
            st.FirstName = request.FirstName;
            st.LastName = request.LastName;
            st.BirthDate = Convert.ToDateTime(request.BirthDate);
            //st.StudiesName = request.StudiesName;

            _dbService.EnrollStudent(request);
            var response = new EnrollStudentResponse();
            response.IndexNumber = st.IndexNumber;
            response.FirstName = st.FirstName;
            response.LastName = st.LastName;
            response.BirthDate = Convert.ToDateTime(st.BirthDate);
            //response.StudiesName = st.StudiesName;
            list.Add(response);
            //return Ok(_dbService.EnrollStudent(request));
            return Ok(list);

        }

         [HttpPost]
         public IActionResult PromoteStudents(int semester, string studiesname)
         {

             return Ok();
         }

        [HttpPost]
        public IActionResult Login(LoginRequestDto request)
        {
            // sprawdzanie hasla w db
            var pass = request.Passw;
            var index = request.IndexNumber;

            if (pass == null && index == null)
            {
                throw new Exception("Index number and password cannot be null.");
            }

            if(index == User.Identity.Name)
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

    }
}