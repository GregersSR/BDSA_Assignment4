using System.Collections.Generic;

namespace Assignment4.Entities
{
    public class Tag
    {
        public int Id { get; set;}
        public string Name { get; set;}
        public ICollection<Task> Task { get; set;}
    }
}
