using Microsoft.AspNetCore.Http;
using myApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.ViewModals
{
    public class EmployeeCreateViewModel
    {
        public string Name { get; set; }
        [Required]
        [Range(0, 2000000)]
        public decimal Salary { get; set; }
        [Required]
        public Department? Department { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public IFormFile Photo {get; set;}
    }
}
