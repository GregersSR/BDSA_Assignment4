using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assignment4.Core;
using System.Linq;

namespace Assignment4.Entities
{
    public class TaskRepository : Assignment4.Core.ITaskRepository
    {
        private readonly KanbanContext context;

        public TaskRepository(KanbanContext context) {
            this.context = context;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            throw new System.NotImplementedException();
        }

        public Response Delete(int taskId)
        {
            throw new System.NotImplementedException();
        }

        public TaskDetailsDTO Read(int taskId)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}