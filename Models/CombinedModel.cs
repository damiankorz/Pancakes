using System.Collections.Generic;

namespace Activities.Models
{
    public class HomeModels 
    {
        public List<ActivityModel> AllActivities {get; set;}
        public User User {get; set;}
    }
    public class ShowActivity 
    {
        public ActivityModel Activity {get; set;}
        public User User {get; set;}
    }
}