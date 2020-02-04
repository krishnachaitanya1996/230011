using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FamilyMVC.Models
{
    public class HouseHoldMemberDetails
    {
        [DisplayName("First Name")]
        [Required]
        [StringLength(32, ErrorMessage = "Max 32 characters long.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-*]*$", ErrorMessage = "Use Characters only")]
        public string FirstName { get; set; }
        [DisplayName("M.I.")]
        public string MiddleName { get; set; }
        [DisplayName("last Name")]
        [Required]
        [StringLength(32, ErrorMessage = "Max 32 characters long.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-*]*$", ErrorMessage = "Use Characters only")]
        public string LastName { get; set; }
        [DisplayName("Suffix")]
        public string Suffix { get; set; }
        [DisplayName("Date Of Birth(mm/dd/yyyy)")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Gender")]
        [Required]
        public string Gender { get; set; }

        public int FamilyId { get; set; }
        public int MemberId { get; set; }

        public int UserId { get; set; }

        public bool? isFirstMember { get; set; }
    }
}