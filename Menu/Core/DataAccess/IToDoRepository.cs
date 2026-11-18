using Menu.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.DataAccess
{
    internal interface IToDoRepository
    {
        //Возвращает все задачи ToDoItem для UserId
        Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct);
        //Возвращает ToDoItem для UserId со статусом Active
        Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct);
        //Возвращает задачи пользователя которые удовлетворяют предикату
        Task<IReadOnlyList<ToDoItem>> Find(Guid userId, Func<ToDoItem, bool> predicate, CancellationToken ct);
        //Возвращает задачу по id
        Task<ToDoItem?> Get(Guid id, CancellationToken ct);
        //Добавление задачи 
        Task Add(ToDoItem item, CancellationToken ct);
        //Обносление задачи
        Task Update(ToDoItem? item, CancellationToken ct);
        //удаление задачи
        Task Delete(Guid id, CancellationToken ct);
        //Проверяет есть ли задача с таким именем у пользователя
        Task<bool> ExistsByName(Guid userId, string name, CancellationToken ct);
        //Возвращает количество активных задач у пользователя
        Task<int> CountActive(Guid userId, CancellationToken ct);
        
    }
}
