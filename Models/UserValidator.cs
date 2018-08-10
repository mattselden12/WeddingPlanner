using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class UserValidator
    {
        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]*$")]
        public string FirstName {get;set;}

        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]*$")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        [Compare("ConfirmPassword")]
        public string Password {get;set;}

        public string ConfirmPassword {get;set;}
    }
}