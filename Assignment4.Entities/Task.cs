namespace Assignment4.Entities
{
    public class Task
    {
        public int Id { get; set;}
        public string Title { get; set;}
        public User AssignedTo { get; set;}
        public string Description { get; set;}
        public State State { get; set;}
        enum State
        {
            New,
            Active,
            Resolved,
            Closed,
            Removed,
        }
        public ICollection<Tag> Tags { get; set;}
    }
}
