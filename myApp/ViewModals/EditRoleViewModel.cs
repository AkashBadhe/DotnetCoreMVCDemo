using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.ViewModals
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            this.Users = new List<string>();
        }
        public string Id { get; set; }
        [Required]
        public string Rolename { get; set; }
        public List<string> Users { get; set; }
    }
}
