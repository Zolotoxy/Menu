using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
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
        //кол-во задач введённое пользователем
        public int InputCountTasks {  get; set; }
        //введённая длина символов задачи
        public int InputLenghtTask { get; set; }
        //Список задач
        private List<ToDoItem> _tasks = new List<ToDoItem>();
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
            if (_tasks.Count >= InputCountTasks)
            {
                throw new ArgumentException($"Превышено максимальное количество задач равное: {InputCountTasks}");
            }
        }
        //проверка на максим длинну символов задачи
        public void isMaxLenghtTask(string? task)
        {
            if (task?.Length >= InputLenghtTask)
            {
                throw new ArgumentException($"Длинна задачи {InputLenghtTask} превышает максимально допустимое значение {MAX_LENGHT_TASK}");
            }
        }
        /// <summary>
        /// Приводим строку к int и проверяет на нахождение в диапазон и выдаём ошибку ели не проходит
        /// </summary>
        public void ParseAndValidateCountInt(string? str)
        {
            if (int.TryParse(str, out int i) && (i > MIN_TASKS && i <= MAX_TASKS))
            {
                InputCountTasks = i;
            }
            else
            {
                throw new ArgumentException("Вы ввели недопустимое значение, пожалуйста попробуйте ещё раз.");
            }
        }
        /// <summary>
        /// Приводим строку к int и проверяет на нахождение в диапазон и выдаём ошибку ели не проходит
        /// </summary>
        public void ParseAndValidateLenghtInt(string? str)
        {
            if (int.TryParse(str, out int i) && (i > MIN_LENGHT_TASK && i <= MAX_LENGHT_TASK))
            {
                InputLenghtTask = i;
            }
            else
            {
                throw new ArgumentException("Вы ввели недопустимое значение, пожалуйста попробуйте ещё раз.");
            }
        }
    }
}
