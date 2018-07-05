using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Activities.Models
{
    // Validation attribute for future date
    public class RestrictedDateTime : ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            DateTime inputDate = (DateTime)date;
            return inputDate > DateTime.Now;
        }
    }
    public class ActivityModel : BaseEntity
    {
        public int ID {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}

        [Required]
        [Display(Name = "Title")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long")]
        public string Title {get; set;}

        [Required]
        [Display(Name = "Time")]
        public DateTime Time {get; set;}

        [Required]
        [RestrictedDateTime(ErrorMessage = "Date must be set in the future")]
        public DateTime Date {get; set;}

        [Required]
        [Display(Name = "Duration")]
        [RegularExpression(@"^?[0-9]\d*(\.\d+)?$", ErrorMessage = "Duration must be an integer or decimal")]
        public string Duration {get; set;}

        [Required]
        [Display(Name = "Description")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
        public string Description {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Participant> Participants {get; set;}
        public ActivityModel()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Participants = new List<Participant>();
        }
    }
}