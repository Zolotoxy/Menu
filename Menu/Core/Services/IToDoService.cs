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
        Task<bool> IsDublicate (Guid userId, string name, CancellationToken ct);
        //проверка максим кол-во задач
        Task IsMaxCountTasks(ToDoUser user, CancellationToken ct);
        //проверка на максим длинну символов задачи
        Task IsMaxLenghtTask(string? task, CancellationToken ct);
        //Возвращает ToDoItem для UserId с любым статусом
        Task <IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct);
        //Возвращает ToDoItem для UserId со статусом Active
        Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct);
        //Добавить задачу
        Task<ToDoItem> Add(ToDoUser user, string name, CancellationToken ct);
        //Отметить задачу "Выполнено"
        Task MarkCompleted(Guid id, CancellationToken ct);
        //Удалить задачу
        Task Delete(Guid id, CancellationToken ct);
        //Возвращает задачу по id
        Task<ToDoItem?> Get(Guid id, CancellationToken ct);
        //Возвращает все задачи пользвателя котор начинаются на namePrefix
        Task<IReadOnlyList<ToDoItem>> Find(ToDoUser user, string namePrefix, CancellationToken ct);
    }
}
