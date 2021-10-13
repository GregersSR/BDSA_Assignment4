using System;
using Xunit;
using Assignment4.Entities;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;

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

        [Fact]
        public void Update_ReturnsUpdated_GivenValidTaskId()
        {
            var task = new TaskUpdateDTO
            {
                Id = 1,
                State = State.Resolved,
            };
            context.Task.Add(new Task {
                Id = 1,
            });
            context.SaveChanges();
            var response = repo.Update(task);
            Assert.Equal(Response.Updated, response);
        }
        [Fact]
        public void Update_ReturnsNotFound_GivenInvalidTaskId()
        {
            var task = new TaskUpdateDTO
            {
                Id = 1,
            };
            var response = repo.Update(task);

            Assert.Equal(Response.NotFound, response);
        }
        [Fact]
        public void Update_UpdatesTaskState_GivenNewState()
        {
            var task = new TaskUpdateDTO
            {
                Id = 1,
                State = State.Resolved,
            };
            context.Task.Add(new Task {
                Id = 1,
            });
            context.SaveChanges();
            repo.Update(task);
            var task1 = repo.Read(1);
            Assert.Equal(State.Resolved, task1.State);
        }
        [Fact]
        public void Read_ReturnsTaskDetailsDTO_GivenValidId()
        {
            var tddto = new TaskDetailsDTO (
                1, "", "", DateTime.Today, "", new List<string>(), State.New, DateTime.Today);
            context.Task.Add(new Task{
                Id = 1,
                Title = "",
                AssignedTo = new User{Id = 1, Name = "", Email = ""},
                Description = "",
                State = State.New
            });
            context.SaveChanges();
            var task = repo.Read(1);

            Assert.Equal(1, task.Id);
            Assert.Equal("", task.Title);
            Assert.Equal("", task.Description);
            Assert.Equal("", task.AssignedToName);
            Assert.Equal(State.New, task.State);
            Assert.Equal(DateTime.Today, task.Created, precision: TimeSpan.FromSeconds(5));
            Assert.Equal(DateTime.Today, task.StateUpdated, precision: TimeSpan.FromSeconds(5));
        }
        [Fact]
        public void ReadAll_ReturnsAListWithAllTask()
        {
        //Given
        var tag = new Tag{ Id =1, Name = "name"};
        var alltaskDTO = new List<TaskDTO>();
        alltaskDTO.Add(new TaskDTO(1,"", "", new List<string>(){tag.Name}, State.New));
            context.AddRange(
            new Task{
                Id = 1,
                Title = "",
                AssignedTo = new User{Id = 1, Name = "", Email = ""},
                Description = "",
                State = State.New,
                Tags = new List<Tag>(){tag}
            });
        context.SaveChanges();
        //When
        var taska = repo.ReadAll().Single();
        var taske = alltaskDTO.Single();
        //Then
        Assert.Equal(taske.Id,taska.Id);
        Assert.Equal(taske.Title,taska.Title);
        Assert.Equal(taske.AssignedToName,taska.AssignedToName);
        Assert.Equal(taske.State,taska.State);
        Assert.Equal(taske.Tags,taska.Tags);
        }

        [Fact]
        public void ReadAll_ReturnsAListWithAllTask_GivenAState()
        {
        //Given
        var tag = new Tag{ Id =1, Name = "name"};
        var alltaskDTO = new List<TaskDTO>();
        alltaskDTO.Add(new TaskDTO(1,"", "", new List<string>(){tag.Name}, State.New));
            context.AddRange(
            new Task{
                Id = 1,
                Title = "",
                AssignedTo = new User{Id = 1, Name = "", Email = ""},
                Description = "",
                State = State.New,
                Tags = new List<Tag>(){tag}
            });
        context.SaveChanges();
        //When
        var taska = repo.ReadAll().Single();
        var taske = alltaskDTO.Single();
        //Then
        Assert.Equal(taske.Id,taska.Id);
        Assert.Equal(taske.Title,taska.Title);
        Assert.Equal(taske.AssignedToName,taska.AssignedToName);
        Assert.Equal(taske.State,taska.State);
        Assert.Equal(taske.Tags,taska.Tags);
        }

        [Fact]
        public void ReadAll_ReturnsAListWithAllTask_GivenTag()
        {
        //Given
        var tag = new Tag{ Id =1, Name = "name"};
        var alltaskDTO = new List<TaskDTO>();
        alltaskDTO.Add(new TaskDTO(1,"", "", new List<string>(){tag.Name}, State.New));
            context.AddRange(
            new Task{
                Id = 1,
                Title = "",
                AssignedTo = new User{Id = 1, Name = "", Email = ""},
                Description = "",
                State = State.New,
                Tags = new List<Tag>(){tag}
            });
        context.SaveChanges();
        //When
        var taska = repo.ReadAll().Single();
        var taske = alltaskDTO.Single();
        //Then
        Assert.Equal(taske.Id,taska.Id);
        Assert.Equal(taske.Title,taska.Title);
        Assert.Equal(taske.AssignedToName,taska.AssignedToName);
        Assert.Equal(taske.State,taska.State);
        Assert.Equal(taske.Tags,taska.Tags);
        }

        [Fact]
        public void ReadAll_ReturnsAListWithAllTask_GivenAnUser()
        {
        //Given
        var tag = new Tag{ Id =1, Name = "name"};
        var alltaskDTO = new List<TaskDTO>();
        alltaskDTO.Add(new TaskDTO(1,"", "", new List<string>(){tag.Name}, State.New));
            context.AddRange(
            new Task{
                Id = 1,
                Title = "",
                AssignedTo = new User{Id = 1, Name = "", Email = ""},
                Description = "",
                State = State.New,
                Tags = new List<Tag>(){tag}
            });
        context.SaveChanges();
        //When
        var taska = repo.ReadAll().Single();
        var taske = alltaskDTO.Single();
        //Then
        Assert.Equal(taske.Id,taska.Id);
        Assert.Equal(taske.Title,taska.Title);
        Assert.Equal(taske.AssignedToName,taska.AssignedToName);
        Assert.Equal(taske.State,taska.State);
        Assert.Equal(taske.Tags,taska.Tags);
        }

        [Fact]
        public void ReadAll_Remove()
        {
        //Given
        var tag = new Tag{ Id =1, Name = "name"};
        var alltaskDTO = new List<TaskDTO>();
        alltaskDTO.Add(new TaskDTO(1,"", "", new List<string>(){tag.Name}, State.New));
            context.AddRange(
            new Task{
                Id = 1,
                Title = "",
                AssignedTo = new User{Id = 1, Name = "", Email = ""},
                Description = "",
                State = State.New,
                Tags = new List<Tag>(){tag}
            });
        context.SaveChanges();
        //When
        var taska = repo.ReadAll().Single();
        var taske = alltaskDTO.Single();
        //Then
        Assert.Equal(taske.Id,taska.Id);
        Assert.Equal(taske.Title,taska.Title);
        Assert.Equal(taske.AssignedToName,taska.AssignedToName);
        Assert.Equal(taske.State,taska.State);
        Assert.Equal(taske.Tags,taska.Tags);
        }

        public void Dispose()
        {
            context.Dispose();
            connection.Dispose();
        }
        
    }
}
