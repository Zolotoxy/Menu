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
        public async Task Add(ToDoItem item, CancellationToken ct)
        {
            _tasks.Add(item);
        }
        //Возвращает количество активных задач у пользователя
        public async Task<int> CountActive(Guid userId, CancellationToken ct)
        {
            var count = _tasks.Where(t => t.User.UserId == userId && t.State == ToDoItemState.Active).Count();
            return count;
        }
        //удаление задачи
        public async Task Delete(Guid id, CancellationToken ct)
        {
            var taskFound = _tasks.Find(t => t.Id == id);
                _tasks.Remove(taskFound);
        }
        //Проверяет есть ли задача с таким именем у пользователя
        public async Task<bool> ExistsByName(Guid userId, string name, CancellationToken ct)
        {
            //Проверка на дубликаты задач
            return _tasks.Exists(t => t.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && t.User.UserId == userId);
        }
        //Возвращает задачи пользователя которые удовлетворяют предикату
        public async Task<IReadOnlyList<ToDoItem>> Find(Guid userId, Func<ToDoItem, bool> predicate, CancellationToken ct)
        {
            return _tasks
                .Where(t => t.User.UserId == userId && predicate(t))
                .ToList();
        }
        //Возвращает задачу по id
        public async Task<ToDoItem?> Get(Guid id, CancellationToken ct)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
        //Возвращает ToDoItem для UserId со статусом Active
        public async Task< IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct)
        {
            return _tasks.FindAll(t => t.User?.UserId == userId && t.State == ToDoItemState.Active);
        }
        //Возвращает все задачи ToDoItem для UserId
        public async Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct)
        {
            return _tasks.FindAll(t => t.User?.UserId == userId);
        }
        //Обносление задачи
        public async Task Update(ToDoItem item, CancellationToken ct)
        {
            item.State = ToDoItemState.Completed;
            item.StateChangedAt = DateTime.Now;
        }
    }
}
