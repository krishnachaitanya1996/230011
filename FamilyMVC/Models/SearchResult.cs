using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMVC.Models
{
    public class SearchResult 
    {
        public List<User> usersDatas { get; set; }
        public List<HouseHoldMemberDetails> houseHoldMemberDetails { get; set; }

        public List<FamilyRelations> FamilyRelations { get; set; }

        public List<FamilyDetails> familyDetails { get; set; }
    }
}