using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Activities.Models
{
    public class User : BaseEntity
    {
        public int ID {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Participant> RSVPS {get; set;}
        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            RSVPS = new List<Participant>();
        }
    }
    public class UserLogin : BaseEntity
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string LoginEmail {get; set;}

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string LoginPassword {get; set;}
    }
    public class UserRegister : BaseEntity
    {
        [Required]
        [Display(Name = "First Name")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "First name must not contain numbers or special characters")]
        public string FirstName {get; set;}

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Last name must not contain numbers or special characters")]
        public string LastName {get; set;}

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email {get; set;}
        // ^(?=.*\d)(?=.*[a-zA-z](?=.*[!@#$%&]))
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-z])(?=.*[!@#$%&]).*$", ErrorMessage = "Password must contain at least 1 letter, 1 number, and 1 special character (!,@,#,$,%,&)")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password {get; set;}

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get; set;}
    }
    public class UserViewModels : BaseEntity
    {
        public UserRegister Register {get; set;}
        public UserLogin Login {get; set;}
    }
}