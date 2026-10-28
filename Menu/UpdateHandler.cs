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
        //отображение команд без авторизации
        const string NOT_NAME = "Введите пожалуйста команду из списка ниже: \n/start\n/help\n/info\n/exit\n";
        //отображение команд после авторизации
        const string IS_NAME = "введите пожалуйста команду из списка ниже: \n/start\n/help\n/info\n/exit\n/addtask\n/showtasks\n/removetask\n/completetask\n/showalltasks";
        //Хранилище пользователей
        private IUserService _userService;
        //Хранилище задач
        private IToDoService _itemService;
        //текущий пользователь
        private ToDoUser _currentUser;
        //Конструктор
        public UpdateHandler(IUserService userService, IToDoService itemService)
        {
            _userService = userService;
            _itemService = itemService;
        }
        public async void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {

            try
            {
                if (_start)
                {
                    
                    botClient.SendMessage(update.Message.Chat, "Здравтсвуйте! " + NOT_NAME);
                    _start = false;
                }
                //Обработка комманд
                _currentUser = ExecuteCommand(_userService, botClient, update, _itemService, _currentUser);
            }
            
            catch (ArgumentException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
        }        
        /// <summary>
        /// Обработка комманд
        /// </summary>
        static ToDoUser ExecuteCommand(IUserService _userService, ITelegramBotClient botClient, Update update, IToDoService _itemService, ToDoUser _currentUser)
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
                //авторизация пользователя
                case "/start":
                    _currentUser = ExecuteStartCommand(_userService, botClient, update, _currentUser);
                    break;

                //получить инструкцию как пользоваться программой
                case "/help":

                    ExecuteHelpCommand(_userService, botClient, update, _currentUser);
                    break;
                //получить информацию о программе
                case "/info":

                    ExecuteInfoCommand(_userService, botClient, update);
                    break;
                //добавить задачи в список
                case "/addtask":

                    ExecuteAddtaskCommand(_userService, botClient, update, _itemService, cmd, _currentUser);
                    break;
                //показать задачи из списка
                case "/showtasks":

                    ExecuteShowActiveTasks(_userService, botClient, update, _itemService, _currentUser);
                    break;
                //удалить задачу из списка
                case "/removetask":

                    ExecuteRemovetaskCommand(botClient, update, _itemService, cmd, _userService);
                    break;
                //Найти задачу по Id
                case "/completetask":

                    ExecuteCompletetaskCommand(cmd, _itemService);
                    break;
                //Показать все задачи с любым статусом
                case "/showalltasks":

                    ExecuteShowAllTasksCommand(_userService, botClient, update, _itemService);
                    break;
                //выход пользователя из программы
                case "/exit":

                    ExecuteExitCommand();
                    break;
                //если ввёл неправильно команду
                default:

                    botClient.SendMessage(update.Message.Chat, "Извините, но такой команды \"" + input + "\" не существует!");
                    break;
            }

            return _currentUser;
        }

        /// <summary>
        /// Проверяем что строка не null не пустая
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
        static ToDoUser ExecuteStartCommand(IUserService _userService, ITelegramBotClient botClient, Update update, ToDoUser _currentUser)
        {
            var user = _userService.GetUser(update.Message.From.Id);
            if (user == null)
            {
                _currentUser = _userService.RegisterUser(update.Message.From.Id, update.Message.From.Username);
            }
            botClient.SendMessage(update.Message.Chat, $"\nДобро пожаловать {_currentUser.TelegramUserName} !\nУважаемый  {_currentUser.TelegramUserName} ," + IS_NAME);
            return _currentUser;
            //return currentUser;
        }
        /// <summary>
        /// Выполнение команды Help
        /// </summary>
        static void ExecuteHelpCommand(IUserService _userService, ITelegramBotClient botClient, Update update, ToDoUser _currentUser)
        {
            var name = _currentUser?.TelegramUserName?? "Пользователь";
            
            botClient.SendMessage(update.Message.Chat, name + " Вот список команд:\n" +
                              "/start: Авторизация, ввод имени пользователя.\n" +
                              "/help: Отображает краткую справочную информацию о том, как пользоваться программой.\n" +
                              "/info: Предоставляет информацию о версии программы и дате её создания.\n" +                              
                              "/addtask \"Название задачи\": Команда доступна после авторизации. Добавление задачи в список.\n" +
                              "/showtasks: Команда доступна после авторизации. Показать задачи в списоке у которых статус \"Актив\".\n" +
                              "/removetask \"Id задачи\": Команда доступна после авторизации. Удалить задачу в списоке.\n" +
                              "/completetask \"Id задачи\": Команда доступна после авторизации. Переводит задачу в статус \"Выполнено\".\n" +
                              "/showalltasks: Команда доступна после авторизации. Показывает задачи с любым статусом.\n" +
                              "/exit: Выход из программы.\n");
        }
        /// <summary>
        /// Выполнение команды Info
        /// </summary>
        static void ExecuteInfoCommand(IUserService _userService, ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat, update.Message.From.Username + " Версия ПО: 1.2.8; Дата создания: 10.10.2026г.\n");
        }
        /// <summary>
        /// Выполнение команды Addtask
        /// </summary>
        static void ExecuteAddtaskCommand(IUserService _userService, ITelegramBotClient botClient, Update update, IToDoService _itemService, string[] cmd, ToDoUser _currentUser)
        {
            if (cmd.Length < 2)
            {
                throw new ArgumentException("Вы не ввели название задачи. Повторите снова.");
            }

            var task = cmd[1];
            //Проверяем
            ValidateString(task);

            //Проверка на установленное количество задач
            _itemService.isMaxCountTasks();

            //Проверка на установленную длинну задачи 
            _itemService.isMaxLenghtTask(task);

            //Проверка на дубликаты задач
            _itemService.isDublicate(task);
            
            //Добавляем задачу в список
            var t = _itemService.Add(_currentUser, task);
            //tasks.Add(t);
            botClient.SendMessage(update.Message.Chat, "\nЗадача \"" + t.Name + "\" добавлена.\n");
        }
        /// <summary>
        /// Выполнение команды Removetask
        /// </summary>
        static void ExecuteRemovetaskCommand(ITelegramBotClient botClient, Update update, IToDoService _itemService, string[] cmd, IUserService _userService)
        { 
            if (cmd.Length < 2)
            {
                throw new ArgumentException("Вы ввели неправильно Id задачи. Повторите снова.");
            }

            var tasks = _itemService.GetAllByUserId(_userService.GetUser(update.Message.From.Id).UserId);
            //Проверка на наличие задач
            if (tasks.Any())
            {
                if (!Guid.TryParse(cmd?[1], out Guid id))
                {
                    throw new ArgumentException($"Некорректный Id {cmd?[1]}.");
                }
                _itemService.Delete(id);
                botClient.SendMessage(update.Message.Chat, $"Задача удалена.");
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вам нечего удалить, ваш список задач пуст!");
            }
        }
        /// <summary>
        /// Отображение задач со статусом "Актив"
        /// </summary>
        static void ExecuteShowActiveTasks(IUserService _userService, ITelegramBotClient botClient, Update update, IToDoService _itemService, ToDoUser _currentUser)
        {
            var tasks = _itemService.GetActiveByUserId(_userService.GetUser(update.Message.From.Id).UserId);
            if (tasks.Any())
            {
                var j = 1;
                foreach (var t in tasks)
                {
                    botClient.SendMessage(update.Message.Chat, $"{j}. {t.Name} - {t.CreatedAt} - {t.Id}");
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
        static void ExecuteCompletetaskCommand(string[]? cmd, IToDoService _itemService)
        {
            if (cmd.Length < 2)
            {
                throw new ArgumentException("Вы ввели неправильно Id задачи. Повторите снова.");
            }

            if (!Guid.TryParse(cmd?[1], out Guid id))
            {
                throw new ArgumentException($"Некорректный Id {cmd?[1]}.");
            }
            _itemService.MarkCompleted(id);
        }
        /// <summary>
        /// Показать все задачи с любым статусом
        /// </summary>
        static void ExecuteShowAllTasksCommand(IUserService _userService, ITelegramBotClient botClient, Update update, IToDoService _itemService)
        {
            var tasks = _itemService.GetAllByUserId(_userService.GetUser(update.Message.From.Id).UserId);
            if (tasks.Any())
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
        static void ExecuteExitCommand()
        {
            ;
        }
    }
}
