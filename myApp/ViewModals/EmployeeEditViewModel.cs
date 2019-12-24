using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myApp.ViewModals
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public string ExistingPhotoPath { get; set; }
        public int Id { get; set; }
    }
}
