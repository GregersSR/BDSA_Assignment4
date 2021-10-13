using System;
using Xunit;
using Assignment4.Entities;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Assignment4.Entities.Tests
{
    public class TagRepositoryTests : IDisposable
    {
        private readonly KanbanContext context;
        private readonly SqliteConnection connection;
        private readonly TagRepository repo;
        public TagRepositoryTests() {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            repo = new TagRepository(context);
        }

        [Fact]
        public void Create_ReturnsCreatedAnd1_GivenTagDTO()
        {
            var (resp, id) = repo.Create(new TagCreateDTO { Name = "MyTag"} );
            Assert.Equal(Response.Created, resp);
            Assert.Equal(1, id);
        }

        [Fact]
        public void Update_ReturnsUpdated_GivenValidId()
        {
            // Arrange
            var (createResp, id) = repo.Create(new TagCreateDTO { Name = "MyTag"} );
            Assert.Equal(Response.Created, createResp);
            Assert.Equal(1, id);

            // Act 
            var updateResp = repo.Update(new TagUpdateDTO { Id = 1, Name = "MyTag - changed name"} );
            Assert.Equal(Response.Updated, updateResp);
        }
        [Fact]
        public void Update_ReturnsNotFound_GivenInvalidId()
        {
            // Act 
            var updateResp = repo.Update(new TagUpdateDTO { Id = 42, Name = "MyTag - changed name"} );
            Assert.Equal(Response.NotFound, updateResp);
        }

        [Fact]
        public void Read_ReturnsTagDTO_GivenValidId()
        {
            // Arrange
            var (createResp, id) = repo.Create(new TagCreateDTO { Name = "MyTag"} );
            Assert.Equal(Response.Created, createResp);
            Assert.Equal(1, id);

            var tag = repo.Read(1);
            Assert.Equal(new TagDTO(1, "MyTag"), tag);
        }

        [Fact]
        public void Read_ReturnsNull_GivenInvalidId()
        {
            var tag = repo.Read(11);
            Assert.Null(tag);
        }

        [Fact]
        public void Delete_ReturnsDeleted_GivenValidId()
        {
            // Arrange
            var (createResp, id) = repo.Create(new TagCreateDTO { Name = "MyTag"} );
            Assert.Equal(Response.Created, createResp);
            Assert.Equal(1, id);
            
            var resp = repo.Delete(1);
            Assert.Equal(Response.Deleted, resp);
        }

        [Fact]
        public void Delete_ReturnsNotFound_GivenInvalidId()
        {
            var resp = repo.Delete(10);
            Assert.Equal(Response.NotFound, resp);
        }

        [Fact]
        public void ReadAll_ReturnsEmptyCollectionForEmptyDB()
        {
            var all = repo.ReadAll();
            Assert.Empty(all);
        }

        [Fact]
        public void ReadAll_ReturnsSingetonCollectionForSingleElement()
        {
            // Arrange
            var (createResp, id) = repo.Create(new TagCreateDTO { Name = "MyTag"} );
            Assert.Equal(Response.Created, createResp);
            Assert.Equal(1, id);

            var collection = repo.ReadAll();
            Assert.Collection(collection, singleItem => Assert.Equal(singleItem, new TagDTO(1, "MyTag")));
        }

        [Fact]
        public void ReadAll_ReturnsMultipleElements()
        {
            // Arrange
            var (createResp, id) = repo.Create(new TagCreateDTO { Name = "MyTag"} );
            Assert.Equal(Response.Created, createResp);
            Assert.Equal(1, id);
            (createResp, id) = repo.Create(new TagCreateDTO { Name = "MyTag2"} );
            Assert.Equal(Response.Created, createResp);
            Assert.Equal(2, id);

            var collection = repo.ReadAll();
            Assert.Collection(collection,
                              item => Assert.Equal(item, new TagDTO(1, "MyTag")),
                              item => Assert.Equal(item, new TagDTO(2, "MyTag2"))
                              );
        }
        
        public void Dispose()
        {
            context.Dispose();
            connection.Dispose();
        }
    }
}
