using Cwiczenie7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenie7.Controllers;
using Cwiczenie7.DTOs.Requests;
using Cwiczenie7.DTOs.Responses;


namespace Cwiczenie7.Services
{
    public interface IStudentDbService
    {
        IEnumerable<Student> GetStudents();
        Student GetStudent(string id);
        IEnumerable<EnrollStudentResponse> EnrollStudent(EnrollStudentRequest r);
        IEnumerable<EnrollStudentResponse> PromoteStudents(int semester, string studiesname);



    }
}
