using System.Collections.Generic;
using Assignment4.Core;
using System;

namespace Assignment4.Entities
{
    public class Task
    {
        public int Id { get; set;}
        public string Title { get; set;}
        public User AssignedTo { get; set;}
        public string Description { get; set;}
        public DateTime Created { get; set;}
        public DateTime StateUpdated { get; set;}
        public State State { get; set;}
        public ICollection<Tag> Tags { get; set;}
    }
}
