using Menu.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    /// <summary>
    /// Интерфейс работы с задачами
    /// </summary>
    internal interface IToDoService
    {
        //Проверка на одинаковые задачи у пользователя
        bool isDublicate (Guid userId, string name);
        //проверка максим кол-во задач
        void isMaxCountTasks(ToDoUser user);
        //проверка на максим длинну символов задачи
        void isMaxLenghtTask(string? task);
        //Возвращает ToDoItem для UserId с любым статусом
        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        //Добавить задачу
        ToDoItem Add(ToDoUser user, string name);
        //Отметить задачу "Выполнено"
        void MarkCompleted(Guid id);
        //Удалить задачу
        void Delete(Guid id);
        //Возвращает задачу по id
        public ToDoItem? Get(Guid id);
        //Возвращает все задачи пользвателя котор начинаются на namePrefix
        IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix);
    }
}
