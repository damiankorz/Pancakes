using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Activities.Models
{
    public class Participant : BaseEntity
    {
        public int ID {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        public int ActivityID {get; set;}
        public ActivityModel Activity {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public Participant()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}