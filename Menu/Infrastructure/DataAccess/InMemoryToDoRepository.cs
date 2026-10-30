using Menu.Core.DataAccess;
using Menu.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Infrastructure.DataAccess
{
    internal class InMemoryToDoRepository : IToDoRepository
    {
        private List<ToDoItem> _tasks = new List<ToDoItem>();
        public void Add(ToDoItem item)
        {
            throw new NotImplementedException();
        }

        public int CountActive(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool ExistsByName(Guid userId, string name)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix)
        {
            throw new NotImplementedException();
        }

        public ToDoItem? Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void Update(ToDoItem item)
        {
            throw new NotImplementedException();
        }
    }
}
