using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace myApp.Utilities
{
    public class ValidateEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidateEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            var emailString = value.ToString().Split('@');
            return emailString[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
