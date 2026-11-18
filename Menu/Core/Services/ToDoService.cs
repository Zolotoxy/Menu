using Menu.Core.DataAccess;
using Menu.Core.Entities;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    internal class ToDoService : IToDoService
    {
        //максимальное количество задач
        public const int MAX_TASKS = 100;
        //миниимальное количество задач
        public const int MIN_TASKS = 1;
        //максимальное количество символов задачи
         public const int MAX_LENGHT_TASK = 100;
        //миниимальное количество символов задачи
        public const int MIN_LENGHT_TASK = 1;
        //Хранилище задач
        private IToDoRepository _itemRepository;

        public ToDoService(IToDoRepository itemRepository)
        {
            _itemRepository = itemRepository;
   
        }
        //Проверка на дубликаты задач
        public async Task<bool> IsDublicate(Guid userId, string name, CancellationToken ct)
        {
            return await _itemRepository.ExistsByName(userId, name, ct);
        }
        //Добавить задачу
        public async Task<ToDoItem> Add(ToDoUser user, string name, CancellationToken ct)
        {
            if (await IsDublicate(user.UserId, name, ct))
            {
                throw new ArgumentException($"Задача \"{name}\" у пользователя с Id \"{user.UserId}\" существует.");
            }
            else
            {
                var item = new ToDoItem(user, name);
                await _itemRepository.Add(item, ct);
                return item;
            }
        }
        //удалить задачу по id
        public async Task Delete(Guid id, CancellationToken ct)
        {
            await _itemRepository.Delete(id, ct);
        }
        //Возвращает ToDoItem для UserId со статусом Active
        public async Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct)
        {
            return await _itemRepository.GetActiveByUserId(userId, ct);            
        }
        //Возвращает ToDoItem для UserId с любым статусом
        public async Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct)
        {
            return await _itemRepository.GetAllByUserId(userId, ct);
        }
        //Отметить задачу "Выполнено"
        public async Task MarkCompleted(Guid id, CancellationToken ct)
        {
            var task = Get(id, ct);
            await _itemRepository.Update(await task, ct);
        }
        //проверка максим кол-во задач
        public async Task IsMaxCountTasks(ToDoUser user, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var tasks = await _itemRepository.GetAllByUserId(user.UserId, ct);
            if (tasks.Count >= MAX_TASKS)
            {
                throw new ArgumentException($"Превышено максимальное количество задач равное: {MAX_TASKS}");
            }
            //return Task.CompletedTask;
        }
        //проверка на максим длинну символов задачи
        public async Task IsMaxLenghtTask(string? task, CancellationToken ct)
        {
            if (task?.Length >= MAX_LENGHT_TASK)
            {
                throw new ArgumentException($"Длинна задачи {task?.Length} превышает максимально допустимое значение {MAX_LENGHT_TASK}");
            }
        }
        //Возвращает задачу по id
        public async Task<ToDoItem?> Get(Guid id, CancellationToken ct)
        {
            var item = await _itemRepository.Get(id, ct);
            if (item is not null)
            {
                return item;
            }
            else
            {
                throw new ArgumentException($"Задачи с номером \"{id}\" не существует.");
            }
        }

        public async Task<IReadOnlyList<ToDoItem>> Find(ToDoUser user, string namePrefix, CancellationToken ct)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _itemRepository.Find(user.UserId,
                                        item => item.Name.StartsWith(namePrefix, StringComparison.OrdinalIgnoreCase), ct);
        }
    }
}
