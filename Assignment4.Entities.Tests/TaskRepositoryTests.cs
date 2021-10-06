using System;
using Xunit;
using Assignment4.Entities;
using Assignment4.Core;
using Assignment4;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        KanbanContext context;

        [Fact]
        public void create_a_task()
        {
            var taskrepo = new TaskRepository(context);

            Assert.Equal(1,1);
        }
    }
}
