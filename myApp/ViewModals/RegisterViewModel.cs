using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using myApp.Utilities;

namespace myApp.ViewModals
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        [ValidateEmailDomain("gmail.com", ErrorMessage = "Invalided domain. Only gmail domain is allowed.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password and password dose not match.")]
        public string ConfirmPassword { get; set; }
        public string City { get; set; }
    }
}
