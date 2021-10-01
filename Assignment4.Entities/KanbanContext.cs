namespace Assignment4.Entities
{
    public class KanbanContext : DbContext
    {
        public DbSet<Tags> Tags { get; set; }
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
    }
}
