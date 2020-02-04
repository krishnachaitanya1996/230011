using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMVC.Models
{
    public class FamilyRelations
    {
        public string Id { get; set; }
        
        public int RelationId { get; set; }
        public string Relation { get; set; }
        public int FamilyId { get; set; }
    }
}