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
        int InputCountTasks { get; set; }
        //введённая длина символов задачи
        int InputLenghtTask { get; set; }
        //Проверка на одинаковые задачи
        void isDublicate(string? task);

        void isMaxCountTasks();

        void isMaxLenghtTask(string? task);
        void ParseAndValidateCountInt(string? str);
        void ParseAndValidateLenghtInt(string? str);

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
