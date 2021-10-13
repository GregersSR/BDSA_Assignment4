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
            var alltasks =  from t in _context.Task
                            select t;
            var alltaskslist = alltasks.ToList().AsReadOnly();
            var alltaskslistDTO = new List<TaskDTO>();
            foreach (var task in alltaskslist)
            {
                alltaskslistDTO.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(c => c.Name).ToList(), task.State));
            }
            return alltaskslistDTO.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
        {
            var alltasks =  from t in _context.Task
                            where t.State == state
                            select t;
            var alltaskslist = alltasks.ToList().AsReadOnly();
            var alltaskslistDTO = new List<TaskDTO>();
            foreach (var task in alltaskslist)
            {
                alltaskslistDTO.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(c => c.Name).ToList(), task.State));
            }
            return alltaskslistDTO.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            var alltasks =  from t in _context.Task
                            where t.Tags.Select(c => c.Name).Contains(tag)
                            select t;
            var alltaskslist = alltasks.ToList().AsReadOnly();
            var alltaskslistDTO = new List<TaskDTO>();
            foreach (var task in alltaskslist)
            {
                alltaskslistDTO.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(c => c.Name).ToList(), task.State));
            }
            return alltaskslistDTO.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            var alltasks =  from t in _context.Task
                            where t.AssignedTo.Id == userId 
                            select t;
            var alltaskslist = alltasks.ToList().AsReadOnly();
            var alltaskslistDTO = new List<TaskDTO>();
            foreach (var task in alltaskslist)
            {
                alltaskslistDTO.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(c => c.Name).ToList(), task.State));
            }
            return alltaskslistDTO.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            var alltasks =  from t in _context.Task
                            where t.State == State.Removed
                            select t;
            var alltaskslist = alltasks.ToList().AsReadOnly();
            var alltaskslistDTO = new List<TaskDTO>();
            foreach (var task in alltaskslist)
            {
                alltaskslistDTO.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(c => c.Name).ToList(), task.State));
            }
            return alltaskslistDTO.AsReadOnly();
        }

        public Response Update(TaskUpdateDTO task)
        {
            var entity = _context.Task.Find(task.Id);
            
            if (entity == null) {
                return Response.NotFound;
            }
            entity.State = task.State;
            
            _context.SaveChanges();

            return Response.Updated;
        }
    }
}