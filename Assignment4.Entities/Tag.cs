namespace Assignment4.Entities
{
    public class Tag
    {
        public int Id { get; set;}
        public string Name { get; set;}
        public ICollection<Task> Task { get; set;}
        Id : int
    Name : string(50), required, unique
    Tasks : many-to-many reference to Task entity
    }
}
