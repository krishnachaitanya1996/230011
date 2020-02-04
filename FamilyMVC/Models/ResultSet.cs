using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMVC.Models
{
    public class ResultSet : HouseHoldMemberDetails
    {
        public string ApplicationId { get; set; }
        public string isApproved { get; set; }
        //public string Id { get; set; }
        //public int RelationId { get; set; }
        //public string Relation { get; set; }
        //public string UserName { get; set; }
        //public string EmailId { get; set; }
        //public string Password { get; set; }
        //public string UserType { get; set; }
    }
}