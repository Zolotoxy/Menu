using Menu.Core.DataAccess;
using Menu.Core.Entities;
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
        //Список задач
        private List<ToDoItem> _tasks = new List<ToDoItem>();
        private IToDoRepository _itemRepository;

        public ToDoService(IToDoRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public void isDublicate(string? task)
        {
            //Проверка на дубликаты задач
            if (_tasks.Exists(t => t.Name.Contains(task, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Задача {task} уже существует", task);
            }            
        }
        public ToDoItem Add(ToDoUser user, string name)
        {
            var item = new ToDoItem(user, name);
            _tasks.Add(item);
            return item;
        }
        //удалить задачу по id
        public void Delete(Guid id)
        {
            var taskFound = _tasks.Find(t => t.Id == id);
            if (taskFound != null)
            {
                _tasks.Remove(taskFound);
            }
            else
            {
                throw new ArgumentException("Задача с таким ID не найдена. пожалуйста попробуйте ещё раз.");
            }
        }
        //Возвращает ToDoItem для UserId со статусом Active
        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            //новый список для отсортировки со статусом "актив" и Id
            List<ToDoItem> filtered = new List<ToDoItem>();

            foreach (var t in _tasks)
            {
                //условие статус "Актив" и "Id"
                if (t.User.UserId == userId && t.State == ToDoItemState.Active)
                {
                    //добавляем совпадение
                    filtered.Add(t);
                }
            }
            return filtered;
            
        }
        //Возвращает ToDoItem для UserId с любым статусом
        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            //новый список для отсортировки по Id
            List<ToDoItem> filtered = new List<ToDoItem>();

            foreach (var t in _tasks)
            {
                //условие "Id"
                if (t.User.UserId == userId)
                {
                    //добавляем совпадение
                    filtered.Add(t);
                }
            }
            return filtered;
        }
        //Отметить задачу "Выполнено"
        public void MarkCompleted(Guid id)
        {
            var t = _tasks.FirstOrDefault(t => t.Id == id);
            t?.State = ToDoItemState.Completed;
            t?.StateChangedAt = DateTime.UtcNow;
        }
        //проверка максим кол-во задач
        public void isMaxCountTasks()
        {
            if (_tasks.Count >= MAX_TASKS)
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
    }
}
