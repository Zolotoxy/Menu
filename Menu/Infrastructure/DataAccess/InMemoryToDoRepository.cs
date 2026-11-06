using Menu.Core.DataAccess;
using Menu.Core.Entities;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Infrastructure.DataAccess
{
    internal class InMemoryToDoRepository : IToDoRepository
    {
        private List<ToDoItem> _tasks = new List<ToDoItem>();
        //Добавление задачи
        public void Add(ToDoItem item)
        {
            _tasks.Add(item);
        }
        //Возвращает количество активных задач у пользователя
        public int CountActive(Guid userId)
        {
            var count = _tasks.Where(t => t.User.UserId == userId && t.State == ToDoItemState.Active).Count();
            return count;
        }
        //удаление задачи
        public void Delete(Guid id)
        {
            var taskFound = _tasks.Find(t => t.Id == id);
                _tasks.Remove(taskFound);
        }
        //Проверяет есть ли задача с таким именем у пользователя
        public bool ExistsByName(Guid userId, string name)
        {
            //Проверка на дубликаты задач
            return _tasks.Exists(t => t.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && t.User.UserId == userId);
        }
        //Возвращает задачи пользователя которые удовлетворяют предикату
        public IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate)
        {
            throw new NotImplementedException();
        }
        //Возвращает все задачи пользвателя котор начинаются на namePrefix
        public IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix)
        {
            throw new NotImplementedException();
        }
        //Возвращает задачу по id
        public ToDoItem? Get(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
        //Возвращает ToDoItem для UserId со статусом Active
        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return _tasks.FindAll(t => t.User.UserId == userId && t.State == ToDoItemState.Active);
        }
        //Возвращает все задачи ToDoItem для UserId
        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return _tasks.FindAll(t => t.User.UserId == userId);
        }
        //Обносление задачи
        public void Update(ToDoItem item)
        {
            item.State = ToDoItemState.Completed;
            item.StateChangedAt = DateTime.Now;
        }
    }
}
