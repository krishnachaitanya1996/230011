using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FamilyMVC.Models
{
    [CustomValidatorAtleastOneField(ErrorMessage = "You must supply at least one value")]
    public class SearchApplication
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("DOB")]
        public string DOB { get; set; }

        [DisplayName("Application ID")]
        public string ApplicationId { get; set; }

        [DisplayName("Application Status")]
        public string status { get; set; }
    }
}