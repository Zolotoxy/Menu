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
        const string IS_NAME = "введите пожалуйста команду из списка ниже: \n/start\n/help\n/info\n/exit\n/addtask\n/showtasks\n/removetask\n/completetask\n/showalltasks\n/report";
        //Хранилище пользователей
        private  IUserService _userService;
        //Хранилище задач
        private IToDoService _itemService;
        //отчёты
        private IToDoReportService _reportService;
        //текущий пользователь
        private ToDoUser _currentUser;
        //_cmd[0] -команда, _cmd[1] -аргумент
        private string[]? _cmd;

        //Конструктор
        public UpdateHandler(IUserService userService, IToDoService itemService, IToDoReportService reportService)
        {
            _userService = userService;
            _itemService = itemService;
            _reportService = reportService;
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
                _currentUser = ExecuteCommand(botClient, update);
            }
            
            catch (ArgumentException ex)
            {
                botClient.SendMessage(update.Message.Chat, ex.Message);
            }
        }        
        /// <summary>
        /// Обработка комманд
        /// </summary>
        public ToDoUser ExecuteCommand(ITelegramBotClient botClient, Update update)
        {

            var input = update.Message.Text;
            
            //проверяем
            ValidateString(input);
            //разбитие строки на команду и аргумент, _cmd[0] -команда, _cmd[1] -аргумент
            _cmd = input.Split(" ", 2);

            //авторизоваться
            switch (_cmd[0].ToLower())
            {
                //авторизация пользователя
                case "/start":
                    _currentUser = ExecuteStartCommand(botClient, update);
                    break;

                //получить инструкцию как пользоваться программой
                case "/help":

                    ExecuteHelpCommand(botClient, update);
                    break;
                //получить информацию о программе
                case "/info":

                    ExecuteInfoCommand(botClient, update);
                    break;
                //добавить задачи в список
                case "/addtask":

                    ExecuteAddtaskCommand(botClient, update);
                    break;
                //показать задачи из списка
                case "/showtasks":

                    ExecuteShowActiveTasks(botClient, update);
                    break;
                //удалить задачу из списка
                case "/removetask":

                    ExecuteRemovetaskCommand(botClient, update);
                    break;
                //Найти задачу по Id
                case "/completetask":

                    ExecuteCompletetaskCommand();
                    break;
                //Показать все задачи с любым статусом
                case "/showalltasks":

                    ExecuteShowAllTasksCommand(botClient, update);
                    break;
                //Статистика по задачам
                case "/report":

                    ExecuteReportTasksCommand(botClient, update);
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
            botClient.SendMessage(update.Message.Chat, "Для дальнейшей работы введите команду.");
            return _currentUser;
        }

        /// <summary>
        /// Проверяем что строка не null не пустая
        /// </summary>
        public void ValidateString(string? str)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Некорректно введёно поле, пожалуйста попробуйте ещё раз.");
            }
        }
        /// <summary>
        /// Выполнение команды Start
        /// </summary>
        public ToDoUser ExecuteStartCommand(ITelegramBotClient botClient, Update update)
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
        public void ExecuteHelpCommand(ITelegramBotClient botClient, Update update)
        {
            var name = _currentUser?.TelegramUserName?? "Пользователь,";
            
            botClient.SendMessage(update.Message.Chat, name + " Вот список команд:\n" +
                              "/start: Авторизация, ввод имени пользователя.\n" +
                              "/help: Отображает краткую справочную информацию о том, как пользоваться программой.\n" +
                              "/info: Предоставляет информацию о версии программы и дате её создания.\n" +                              
                              "/addtask \"Название задачи\": Команда доступна после авторизации. Добавление задачи в список.\n" +
                              "/showtasks: Команда доступна после авторизации. Показать задачи в списоке у которых статус \"Актив\".\n" +
                              "/removetask \"Id задачи\": Команда доступна после авторизации. Удалить задачу в списоке.\n" +
                              "/completetask \"Id задачи\": Команда доступна после авторизации. Переводит задачу в статус \"Выполнено\".\n" +
                              "/showalltasks: Команда доступна после авторизации. Показывает задачи с любым статусом.\n" +
                              "/report: Команда доступна после авторизации. Показывает статистику по задачам.\n" +
                              "/exit: Выход из программы.\n");
        }
        /// <summary>
        /// Выполнение команды Info
        /// </summary>
        public void ExecuteInfoCommand(ITelegramBotClient botClient, Update update)
        {
            var name = _currentUser?.TelegramUserName ?? "Пользователь,";
            botClient.SendMessage(update.Message.Chat, name + " Версия ПО: 1.3.3; Дата создания: 10.10.2026г.\n");
        }
        /// <summary>
        /// Выполнение команды Addtask
        /// </summary>
        public void ExecuteAddtaskCommand(ITelegramBotClient botClient, Update update)
        {
            if (_cmd.Length < 2)
            {
                throw new ArgumentException("Вы не ввели название задачи. Повторите снова.");
            }

            var task = _cmd[1];
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
        public void ExecuteRemovetaskCommand(ITelegramBotClient botClient, Update update)
        { 
            if (_cmd.Length < 2)
            {
                throw new ArgumentException("Вы ввели неправильно Id задачи. Повторите снова.");
            }

            var tasks = _itemService.GetAllByUserId(_userService.GetUser(update.Message.From.Id).UserId);
            //Проверка на наличие задач
            if (tasks.Any())
            {
                if (!Guid.TryParse(_cmd?[1], out Guid id))
                {
                    throw new ArgumentException($"Некорректный Id {_cmd?[1]}.");
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
        public void ExecuteShowActiveTasks(ITelegramBotClient botClient, Update update)
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
        public void ExecuteCompletetaskCommand()
        {
            if (_cmd.Length < 2)
            {
                throw new ArgumentException("Вы ввели неправильно Id задачи. Повторите снова.");
            }

            if (!Guid.TryParse(_cmd?[1], out Guid id))
            {
                throw new ArgumentException($"Некорректный Id {_cmd?[1]}.");
            }
            _itemService.MarkCompleted(id);
        }
        /// <summary>
        /// Показать все задачи с любым статусом
        /// </summary>
        public void ExecuteShowAllTasksCommand(ITelegramBotClient botClient, Update update)
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
        /// Показать статистику по задачам
        /// </summary>
        public void ExecuteReportTasksCommand(ITelegramBotClient botClient, Update update)
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
        public void ExecuteExitCommand()
        {
            ;
        }
    }
}
