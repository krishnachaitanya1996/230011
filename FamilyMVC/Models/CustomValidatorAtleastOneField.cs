using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FamilyMVC.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomValidatorAtleastOneField : ValidationAttribute
    {
        public override bool IsValid(object value)
        {           
            var typeInfo = value.GetType();
            var propertyInfo = typeInfo.GetProperties();
            foreach (var property in propertyInfo)
            {
                if (null != property.GetValue(value, null))
                {                   
                    return true;
                }
            }
            return false;
        }
    }
}