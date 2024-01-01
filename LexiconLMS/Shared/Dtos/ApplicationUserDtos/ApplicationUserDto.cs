using LexiconLMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Dtos.ApplicationUserDtos
{
    public class ApplicationUserDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Email { get; set;}

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public Course Course { get; set; }
    }
}
