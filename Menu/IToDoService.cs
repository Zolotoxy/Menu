using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    /// <summary>
    /// Интерфейс работы с задачами
    /// </summary>
    internal interface IToDoService
    {
        //Проверка на одинаковые задачи
        void isDublicate(string? task);
        //проверка максим кол-во задач
        void isMaxCountTasks();
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
    }
}
