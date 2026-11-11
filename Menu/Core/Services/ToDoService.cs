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
        //Статистика
       // private IToDoReportService _reportService;

        public ToDoService(IToDoRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        //Проверка на дубликаты задач
        public bool isDublicate(Guid userId, string name)
        {
            return _itemRepository.ExistsByName(userId, name);
        }
        //Добавить задачу
        public ToDoItem Add(ToDoUser user, string name)
        {
            if (isDublicate(user.UserId, name))
            {
                throw new ArgumentException($"Задача \"{name}\" у пользователя с Id \"{user.UserId}\" существует.");
            }
            else
            {
                var item = new ToDoItem(user, name);
                _itemRepository.Add(item);
                return item;
            }
        }
        //удалить задачу по id
        public void Delete(Guid id)
        {
            _itemRepository.Delete(id);
        }
        //Возвращает ToDoItem для UserId со статусом Active
        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return _itemRepository.GetActiveByUserId(userId);            
        }
        //Возвращает ToDoItem для UserId с любым статусом
        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return _itemRepository.GetAllByUserId(userId);
        }
        //Отметить задачу "Выполнено"
        public void MarkCompleted(Guid id)
        {
            var task = Get(id);
            _itemRepository.Update(task);
        }
        //проверка максим кол-во задач
        public void isMaxCountTasks(ToDoUser user)
        {
            if (_itemRepository.GetAllByUserId(user.UserId).Count >= MAX_TASKS)
            {
                throw new ArgumentException($"Превышено максимальное количество задач равное: {MAX_TASKS}");
            }
        }
        //проверка на максим длинну символов задачи
        public void isMaxLenghtTask(string? task)
        {
            if (task?.Length >= MAX_LENGHT_TASK)
            {
                throw new ArgumentException($"Длинна задачи {task?.Length} превышает максимально допустимое значение {MAX_LENGHT_TASK}");
            }
        }
        //Возвращает задачу по id
        public ToDoItem? Get(Guid id)
        {
            var item = _itemRepository.Get(id);
            if (item is not null)
            {
                return item;
            }
            else
            {
                throw new ArgumentException($"Задачи с номером \"{id}\" не существует.");
            }
        }
    }
}
