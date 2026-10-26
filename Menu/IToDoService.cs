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
        //введённая кол-во задач
        int InputCountTasks { get; set; }
        //введённая длина символов задачи
        int InputLenghtTask { get; set; }
        //Проверка на одинаковые задачи
        void isDublicate(string? task);
        //проверка максим кол-во задач
        void isMaxCountTasks();
        //проверка на максим длинну символов задачи
        void isMaxLenghtTask(string? task);
        /// <summary>
        /// Приводим строку к int и проверяет на нахождение в диапазон и выдаём ошибку ели не проходит
        /// </summary>
        void ParseAndValidateCountInt(string? str);
        /// <summary>
        /// Приводим строку к int и проверяет на нахождение в диапазон и выдаём ошибку ели не проходит
        /// </summary>
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
