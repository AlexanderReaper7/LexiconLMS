using LexiconLMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiconLMS.Shared.Dtos.ApplicationUserDtos
{
    public class ApplicationUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Email { get; set;}
        [Required]
        public string Role { get; set; }
        public Course Course { get; set; }
    }

    public class ApplicationUserDtoUpdate : ApplicationUserDto
    {       
        public string OldRole { get; set; }
    }

    public class ApplicationUserDtoAdd : ApplicationUserDto
    {
        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
         ErrorMessage = "Password should be at least 8 character that contains one uppercase, one digit and one special character at least.")]
        public string Password { get; set; }
    }
}
