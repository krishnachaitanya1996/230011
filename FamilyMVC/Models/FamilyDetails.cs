using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMVC.Models
{
    public class FamilyDetails
    {

        public int FamilyId { get; set; }
       
        public int ApplicationId { get; set; }

        public bool isApproved { get; set; }
    }
}