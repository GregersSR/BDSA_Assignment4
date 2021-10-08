using Assignment4.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment4.Entities
{
    public class KanbanContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<User> User { get; set; }

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Task>()
                .Property(e => e.State)
                .HasConversion(new EnumToStringConverter<State>());
        }

        public static void seed(KanbanContext context) 
        {
            var task1 = new Task{ Id = 1, Title = "Work", Description = "Work Hard"};
            context.User.AddRange(
                new User { Id = 1, Name = "Tue", Email = "tugy@itu.dk"}
            );
        }
    }
}
