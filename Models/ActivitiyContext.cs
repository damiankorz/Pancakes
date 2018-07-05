using Microsoft.EntityFrameworkCore;

namespace Activities.Models
{
    public class ActivityContext : DbContext
    {
        public ActivityContext(DbContextOptions<ActivityContext> options) : base(options) {}
        public DbSet<User> Users {get; set;}
        public DbSet<ActivityModel> Activities {get; set;}
        public DbSet<Participant> Participants {get; set;}
    }
}