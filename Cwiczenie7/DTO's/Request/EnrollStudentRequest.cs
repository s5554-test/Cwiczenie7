using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenie7.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required]
        public string IndexNumber { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string StudiesName { get; set; }
    }
}
