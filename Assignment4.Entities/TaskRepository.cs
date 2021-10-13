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
            var tagnamelist = task.Tags.ToList();
            var taglist = new List<Tag>();
            foreach (var t in tagnamelist)
            {
                taglist.Add(new Tag{Name = t});
            }
            var user = _context.User.Find(task.AssignedToId);
            if (user == null)
            {
                return (Response.BadRequest, 0);
            }
            var entity = new Task {
                Title = task.Title,
                AssignedTo = user,
                Description = task.Description,
                Created = System.DateTime.UtcNow,
                StateUpdated = System.DateTime.UtcNow,
                Tags = taglist,
            }; 
            _context.Task.Add(entity);
            _context.SaveChanges();
            return (Response.Created, entity.Id);
        }
        public Response Delete(int taskId)
        {
            var entity = _context.Task.Find(taskId);
            if (entity == null)
            {
                return Response.NotFound;
            }
            if (entity.State == State.Resolved || entity.State == State.Closed || entity.State == State.Removed)
            {
                return Response.Conflict;
            } else if (entity.State == State.New) {
                _context.Task.Remove(entity);
                _context.SaveChanges();
                return Response.Deleted;
            } else if (entity.State == State.Active)
            {
                entity.State = State.Removed;
                return Response.Deleted;
            }
            return Response.NotFound;

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
            entity.StateUpdated = System.DateTime.UtcNow;
            var tagnamelist = task.Tags.ToList();
            var taglist = new List<Tag>();
            foreach (var t in tagnamelist)
            {
                taglist.Add(new Tag{Name = t});
            }
            entity.Tags = taglist;
            _context.SaveChanges();

            return Response.Updated;
        }
    }
}