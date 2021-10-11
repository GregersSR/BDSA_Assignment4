using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assignment4.Core;
using System.Linq;

namespace Assignment4.Entities
{
    public class TaskRepository : Assignment4.Core.ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context) {
            this._context = context;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            _context.Task.Add(new Task {
                Title = task.Title,
                Description = task.Description,
            });
            return (Response.Created, _context.SaveChanges());
        }

        public Response Delete(int taskId)
        {
            var entity = _context.Task.Find(taskId);
            if (entity == null) {
                return Response.NotFound;
            } else {
                _context.Task.Remove(entity);
                _context.SaveChanges();
                return Response.Deleted;
            }

        }
        public TaskDetailsDTO Read(int taskId)
        {
            var task =  from t in _context.Task
                        where t.Id == taskId
                        select new TaskDetailsDTO(
                            t.Id,
                            t.Title,
                            t.Description,
                            System.DateTime.Today,
                            t.AssignedTo.Name,
                            t.Tags.Select(t => t.Name).ToList(),
                            t.State,
                            System.DateTime.Today
                        );
            return task.FirstOrDefault();
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new System.NotImplementedException();
        }

        public Response Update(TaskUpdateDTO task)
        {
            var entity = _context.Task.Find(task.Id);
            
            if (entity == null) {
                return Response.NotFound;
            }
            
            entity.Id = task.Id;
            
            _context.SaveChanges();

            return Response.Updated;
        }
    }
}