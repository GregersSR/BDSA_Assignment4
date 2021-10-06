using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assignment4.Core;
using System.Linq;

namespace Assignment4.Entities
{
    public class TaskRepository : Assignment4.Core.ITaskRepository
    {
        KanbanContext context;

        public TaskRepository(KanbanContext context) {
            this.context = context;
        }
        
        public IReadOnlyCollection<TaskDTO> All() {
            var query = from c in context.Task select c;
            return (ReadOnlyCollection<TaskDTO>) query.AsEnumerable();
        }
        public int Create(TaskDTO task) 
        {
            context.Add(task);
            return task.Id;
        }

        public void Delete(int taskId) 
        {
            var query = from t in context.Task where t.Id == taskId select t;
            context.Remove(query);
        }

        public TaskDetailsDTO FindById(int id) 
        {
            var query = from t in context.Task where t.Id == id select t;
            return (TaskDetailsDTO) query;
        }

        public void Update(TaskDTO task) 
        {
            context.Update(task);
        }

        public void Dispose() {
            context.Dispose();
        }
    }
}