using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    internal interface IToDoRepository
    {
        //Возвращает все задачи ToDoItem для UserId
        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        //Возвращает задачи пользователя которые удовлетворяют предикату
        IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate);
        //Возвращает все задачи пользвателя котор начинаются на namePrefix
        IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix);
        ToDoItem? Get(Guid id);
        void Add(ToDoItem item);
        void Update(ToDoItem item);
        void Delete(Guid id);
        //Проверяет есть ли задача с таким именем у пользователя
        bool ExistsByName(Guid userId, string name);
        //Возвращает количество активных задач у пользователя
        int CountActive(Guid userId);
        
    }
}
