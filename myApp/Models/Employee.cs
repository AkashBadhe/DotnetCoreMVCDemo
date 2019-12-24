using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 2000000)]
        public decimal Salary { get; set; }
        [Required]
        public Department? Department { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string PhotoPath { get; set; }
    }
}
