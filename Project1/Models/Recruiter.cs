using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class Recruiter
    {
        public int RecruiterID { get; set; }
        public string RecruiterEmail { get; set; }

        //[Required(ErrorMessage = "Please Enter Comments. Thanks for your honest feedback")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Please enter a valid alphanumeric entry")]
        public string Comments { get; set; }
    }
}
