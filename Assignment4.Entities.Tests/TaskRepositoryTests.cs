using System;
using Xunit;
using Assignment4.Entities;
using Assignment4.Core;
using Assignment4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        private readonly KanbanContext context;
        private readonly TaskRepository repo;
        public TaskRepositoryTests() {
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            using var _context = new KanbanContext(builder.Options);
            _context.Database.EnsureCreated();
            context = _context;
            repo = new TaskRepository(context);
        }
    }
}
