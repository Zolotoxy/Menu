using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace Menu
{
    
    internal class UpdateHandler : IUpdateHandler
    {
        //флаг старта программы
        private bool _start = true;
        //максимальное количество задач
        const int MAX_TASKS = 100;
        //миниимальное количество задач
        const int MIN_TASKS = 1;
        //максимальное количество символов задачи
        const int MAX_LENGHT_TASK = 100;
        //миниимальное количество символов задачи
        const int MIN_LENGHT_TASK = 1;
        //отображение команд без авторизации
        const string NOT_NAME = "Введите пожалуйста команду из списка ниже: \n/start\n/help\n/info\n/exit\n";
        //отображение команд после авторизации
        const string IS_NAME = "введите пожалуйста команду из списка ниже: \n/start\n/help\n/info\n/exit\n/addtask\n/showtasks\n/removetask\n/completetask\n/showalltasks";
        //введённое имя пользователя
        private ToDoUser? _currentUser = null;
        //кол-во задач введённое пользователем
        private int _inputCountTasks = 0;
        //введённая длина символов задачи
        private int _inputLenghtTask = 0;
        //Список задач
        private List<ToDoItem> _tasks = new List<ToDoItem>();
        private readonly IUserService _service;

        public UpdateHandler(IUserService service)
        {
            _service = service;
        }
        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {
            try
            {   //Инициализация параметров при старте
                InputAndInutialyzeParametrs(ref _inputCountTasks, ref _inputLenghtTask, botClient, update);

                //Обработка комманд
                /*_currentUser = */ExecuteCommand(_service, _tasks, ref _start, _inputCountTasks, _inputLenghtTask, botClient, update);
            }
            catch (TaskCountLimitException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message + ": " + ex.TaskCountLimit);
            }
            catch (TaskLengthLimitException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message + ": " + ex.TaskLenghtLimit);
            }
            catch (DublicateTaskException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message + ": " + ex.Task);
            }
            catch (ArgumentException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
            
        }
        /// <summary>
        /// Инициализация кол-во задач и длинны задачи при старте программы 
        /// </summary>
        static void InputAndInutialyzeParametrs(ref int inputCountTasks, ref int inputLenghtTask, ITelegramBotClient botClient, Update update)
        {
            if (inputCountTasks == 0)
            {
                botClient.SendMessage(update.Message.Chat, "Введите максимальное допустимое количество задач от 1 до 100:");
                var str = ReadLine();
                inputCountTasks = ParseAndValidateInt(str, MIN_TASKS, MAX_TASKS);
            }

            if (inputLenghtTask == 0)
            {
                botClient.SendMessage(update.Message.Chat, "Введите максимально допустимую длинну символов задачи от 1 до 100:");
                var str = ReadLine();
                inputLenghtTask = ParseAndValidateInt(str, MIN_LENGHT_TASK, MAX_LENGHT_TASK);

                //приветствие
                botClient.SendMessage(update.Message.Chat, "Здравтсвуйте! " + NOT_NAME);
            }
        }
        /// <summary>
        /// Обработка комманд
        /// </summary>
        static void ExecuteCommand(IUserService _service, List<ToDoItem> tasks, ref bool start, int inputCountTasks, int inputLenghtTask, ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat, "Для дальнейшей работы введите команду.");
            var input = ReadLine();
            //проверяем
            ValidateString(input);
            //разбитие строки на команду и аргумент, cmd[0] -команда, cmd[1] -аргумент
            var cmd = input.Split(" ", 2);

            //авторизоваться
            switch (cmd[0].ToLower())
            {

                case "/start":
                    //if (currentUser != null)
                    //{
                    //    botClient.SendMessage(update.Message.Chat, $"Вы уже зарегестрированы как {currentUser.TelegramUserName}.");
                    //    break;
                    //}
                    ExecuteStartCommand(_service, botClient, update);
                    break;

                //получить инструкцию как пользоваться программой
                case "/help":

                    ExecuteHelpCommand(_service, botClient, update);
                    break;
                //получить информацию о программе
                case "/info":

                    ExecuteInfoCommand(_service, botClient, update);
                    break;
                //добавить задачи в список
                case "/addtask":

                    ExecuteAddtaskCommand(tasks, _service, inputCountTasks, inputLenghtTask, botClient,update);
                    break;
                //показать задачи из списка
                case "/showtasks":

                    ExecuteShowtasksCommand(tasks, botClient, update);
                    break;
                //удалить задачу из списка
                case "/removetask":

                    ExecuteRemovetaskCommand(tasks, inputCountTasks, botClient, update);
                    break;
                //Найти задачу по Id
                case "/completetask":

                    ExecuteCompletetaskCommand(tasks, cmd);
                    break;
                //Показать все задачи с любым статусом
                case "/showalltasks":

                    ExecuteShowAllTasksCommand(tasks, botClient, update);
                    break;
                //выход пользователя из программы
                case "/exit":

                    ExecuteExitCommand(ref start);
                    break;
                //если ввёл неправильно команду
                default:

                    botClient.SendMessage(update.Message.Chat, "Извините, но такой команды \"" + input + "\" не существует!");
                    break;
            }
            //return currentUser;
        }
        /// <summary>
        /// Приводим строку к int и проверяет на нахождение в диапазон и выдаём ошибку ели не проходит
        /// </summary>
        static int ParseAndValidateInt(string? str, int min, int max)
        {
            if (int.TryParse(str, out int i) && (i > min && i <= max))
            {
                return i;
            }
            else
            {
                throw new ArgumentException("Вы ввели недопустимое значение, пожалуйста попробуйте ещё раз.");
            }
        }
        /// <summary>
        /// Проверяем что строка не null не пустая и обрезаем пробелы
        /// </summary>
        static void ValidateString(string? str)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Некорректно введёно поле, пожалуйста попробуйте ещё раз.");
            }
        }
        /// <summary>
        /// Выполнение команды Start
        /// </summary>
        static void ExecuteStartCommand(IUserService _service, ITelegramBotClient botClient, Update update)
        {

            //botClient.SendMessage(update.Message.Chat, "Введите ваше имя:");
            //ввод имени
            //var name = ReadLine();
            //Проверяем
            //ValidateString(name);
            //currentUser = new ToDoUser(name);
            var user = _service.GetUser(update.Message.Id);
            if (user == null)
            {
                _service.RegisterUser(update.Message.From.Id, update.Message.From.Username);
            }
            botClient.SendMessage(update.Message.Chat, $"\nДобро пожаловать {update.Message.From.Username} !\nУважаемый  {update.Message.From.Username} ," + IS_NAME);
            //return currentUser;
        }
        /// <summary>
        /// Выполнение команды Help
        /// </summary>
        static void ExecuteHelpCommand(IUserService _service, ITelegramBotClient botClient, Update update)
        {
            var name = "";
            if (update.Message.From.Username == null)
            {
                name = update.Message.From.Username;
            }

            botClient.SendMessage(update.Message.Chat, name + " Вот список команд:\n" +
                              "/start: Авторизация, ввод имени пользователя.\n" +
                              "/help: Отображает краткую справочную информацию о том, как пользоваться программой.\n" +
                              "/info: Предоставляет информацию о версии программы и дате её создания.\n" +
                              "/echo: Команда доступна после авторизации. При вводе этой команды с аргументом \"/echo Hello\" программа возвращает текст \"Hello\")\n" +
                              "/addtask: Команда доступна после авторизации. Добавление задачи в список.\n" +
                              "/showtasks: Команда доступна после авторизации. Показать задачи в списоке у которых статус \"Актив\".\n" +
                              "/removetask: Команда доступна после авторизации. Удалить задачу в списоке.\n" +
                              "/completetask: Команда доступна после авторизации. Переводит задачу в статус \"Выполнено\".\n" +
                              "/showalltasks: Команда доступна после авторизации. Показывает задачи с любым статусом.\n" +
                              "/exit: Выход из программы.\n");
        }
        /// <summary>
        /// Выполнение команды Info
        /// </summary>
        static void ExecuteInfoCommand(IUserService _service, ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat, update.Message.From.Username + " Версия ПО: 1.2.3; Дата создания: 10.10.2026г.\n");
        }
        /// <summary>
        /// Выполнение команды Addtask
        /// </summary>
        static void ExecuteAddtaskCommand(List<ToDoItem> tasks, IUserService _service, int inputCountTasks, int inputLenghtTask, ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat, "Пожалуйста, введите описание задачи:");
            var task = ReadLine();
            //Проверяем
            ValidateString(task);

            //Проверка на установленное количество задач
            if (tasks.Count >= inputCountTasks)
            {
                throw new TaskCountLimitException($"Превышено максимальное количество задач равное: {inputCountTasks}", inputCountTasks);
            }

            //Проверка на установленную длинну задачи 
            if (task.Length >= inputLenghtTask)
            {
                throw new TaskLengthLimitException($"Длинна задачи {inputLenghtTask} превышает максимально допустимое значение {MAX_LENGHT_TASK}", inputLenghtTask, MAX_LENGHT_TASK);
            }

            //Проверка на дубликаты задач
            if (tasks.Exists(t => t.Name.Contains(task, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DublicateTaskException($"Задача {task} уже существует", task);
            }

            //Добавляем задачу в список
            var t = new ToDoItem(currentUser, task);
            tasks.Add(t);
            botClient.SendMessage(update.Message.Chat, "\nЗадача \"" + t.Name + "\" добавлена.\n");
        }
        /// <summary>
        /// Выполнение команды Showtasks
        /// </summary>
        static void ExecuteShowtasksCommand(List<ToDoItem> tasks, ITelegramBotClient botClient, Update update)
        {
            if (tasks.Count >= MIN_TASKS)
            {
                ExecuteShowActiveTasks(tasks, botClient, update);
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Ваш список задач пуст! Добавте задачи.");
            }
        }
        /// <summary>
        /// Выполнение команды Removetask
        /// </summary>
        static void ExecuteRemovetaskCommand(List<ToDoItem> tasks, int inputCountTasks, ITelegramBotClient botClient, Update update)
        {
            //Проверка на наличие задач
            if (tasks.Count >= MIN_TASKS)
            {
                botClient.SendMessage(update.Message.Chat, "Вот ваш список задач:\n");

                ExecuteShowActiveTasks(tasks, botClient, update);

                botClient.SendMessage(update.Message.Chat, "Введите номер задачи для удаления:\n");
                var str = ReadLine();
                var numberTask = ParseAndValidateInt(str, 0, inputCountTasks);
                //удаляем задачу из списка
                botClient.SendMessage(update.Message.Chat, $"Задача {tasks[numberTask -= 1].Name} удалена.");
                tasks.RemoveAt(numberTask);
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вам нечего удалить, ваш список задач пуст!");
            }
        }
        /// <summary>
        /// Отображение задач со статусом "Актив"
        /// </summary>
        static void ExecuteShowActiveTasks(List<ToDoItem> tasks, ITelegramBotClient botClient, Update update)
        {
            if (tasks.Count >= MIN_TASKS)
            {
                var j = 1;
                foreach (var t in tasks)
                {
                    if (t.State == ToDoItemState.Active)
                    {
                        botClient.SendMessage(update.Message.Chat, $"{j}. {t.Name} - {t.CreatedAt} - {t.Id}");
                    }
                    j++;
                }
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Ваш список задач пуст! Добавте задачи.");
            }

        }
        /// <summary>
        /// Найти задачу по ID
        /// </summary>
        static void ExecuteCompletetaskCommand(List<ToDoItem> tasks, string[]? cmd)
        {
            if (!Guid.TryParse(cmd?[1], out Guid id))
            {
                throw new ArgumentException($"Некорректный Id {cmd?[1]}.");
            }

            var t = tasks.FirstOrDefault(t => t.Id == id);
            t?.State = ToDoItemState.Completed;
            t?.StateChangedAt = DateTime.UtcNow;

        }
        /// <summary>
        /// Показать все задачи с любым статусом
        /// </summary>
        static void ExecuteShowAllTasksCommand(List<ToDoItem> tasks, ITelegramBotClient botClient, Update update)
        {
            if (tasks.Count >= MIN_TASKS)
            {
                var j = 1;
                foreach (var t in tasks)
                {
                    botClient.SendMessage(update.Message.Chat, $"{j}. ({t.State}) {t.Name} - {t.CreatedAt} - {t.Id}");
                    j++;
                }
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Ваш список задач пуст! Добавте задачи.");
            }
        }
        /// <summary>
        /// Выполнение команды Exit
        /// </summary>
        static void ExecuteExitCommand(ref bool start)
        {
            start = false;
        }
    }
}
