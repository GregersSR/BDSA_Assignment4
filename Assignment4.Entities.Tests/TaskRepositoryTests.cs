using System;
using Xunit;
using Assignment4.Entities;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly KanbanContext context;
        private readonly TaskRepository repo;
        public TaskRepositoryTests() {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            repo = new TaskRepository(context);
        }

        [Fact]
        public void Create_ReturnsResponseAndId_GivenTask()
        {
        var (resp, taskId) = repo.Create(new TaskCreateDTO{
            Title = "Title",
            AssignedToId = 1,
            Description = "description",
            Tags = new List<string> {"one", "two"}
        });
        Assert.Equal((Response.Created, 1),(resp, taskId));
        }
        [Fact]
        public void Delete_ReturnsDeleted_GivenValidTaskId()
        {
        context.Task.Add(new Task {
            Id = 1,
        });
        var response = repo.Delete(1);
        Assert.Equal(Response.Deleted, response);
        }
        [Fact]
        public void Delete_ReturnsNotFound_GivenInvalidId()
        {
            var response = repo.Delete(1);
            Assert.Equal(Response.NotFound, response);
        }
        /* Testing For Read()
        [Fact]
        public void Read_ReturnsTaskDetailsDTO_GivenValidId()
        {
            context.Task.Add(new Task {
                Id = 1,
                Title = "",
                Description = "",
                AssignedTo = null,
                State = State.Active,
                Tags = null,
            });
            var task = repo.Read(1);
            Assert.Equal(1, task.Id);
            Assert.Equal("", task.Title);
            Assert.Equal("", task.Description);
            Assert.Null(task.AssignedToName);
            Assert.Equal(State.Active.ToString(),task.State.ToString());
            Assert.Equal(DateTime.UtcNow, task.Created, precision: TimeSpan.FromSeconds(5));
            Assert.Equal(DateTime.UtcNow);
        }
        */

        public void Dispose()
        {
            context.Dispose();
            connection.Dispose();
        }
    }
}
