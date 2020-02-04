using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FamilyMVC.Models
{
    public class User
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string EmailId { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public string UserType { get; set; }
        [Compare("Password")]
        [MinLength(6)]
        public string ConfirmPassword { get; set; }

        public int UserId { get; set; }
    }
}