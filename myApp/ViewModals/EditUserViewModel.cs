using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace myApp.ViewModals
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string City { get; set; }
    }
}
