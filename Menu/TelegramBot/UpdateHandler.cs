using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using Menu.Core.Entities;
using Menu.Core.Services;


namespace Menu.TelegramBot
{
    
    internal class UpdateHandler : IUpdateHandler
    {
        //флаг старта программы
        private bool _start = true;
        //отображение команд без авторизации
        const string NOT_NAME = "Введите пожалуйста команду из списка ниже: \n/start\n/help\n/info\n/exit\n";
        //отображение команд после авторизации
        const string IS_NAME = "введите пожалуйста команду из списка ниже: \n/start\n/help\n/info\n/exit\n/addtask\n/showtasks\n/removetask\n/completetask\n/showalltasks\n/report\n/find";
        //Хранилище пользователей
        private  IUserService _userService;
        //Хранилище задач
        private IToDoService _itemService;
        //Статистика пользователя по задачам
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
                    ExecuteStartCommand(botClient, update);
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

                    ExecuteCompletetaskCommand(botClient, update);
                    break;
                //Показать все задачи с любым статусом
                case "/showalltasks":

                    ExecuteShowAllTasksCommand(botClient, update);
                    break;
                //Статистика по задачам
                case "/report":

                    ExecuteReportTasksCommand(botClient, update);
                    break;
                //Показывает все задачи по предикату или имени
                case "/find":

                    ExecuteFindTasksCommand(botClient, update);
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
        public void ExecuteStartCommand(ITelegramBotClient botClient, Update update)
        {
            //var user = _userService.GetUser(update.Message.From.Id);
            if (_currentUser is not null)
            {
                botClient.SendMessage(update.Message.Chat, $"\nДобро пожаловать {_currentUser.TelegramUserName} !\nУважаемый  {_currentUser.TelegramUserName} ," + IS_NAME);
            }
            else
            {
                _currentUser = _userService.RegisterUser(update.Message.From.Id, update.Message.From.Username);
            }
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
                              "/find \"Имя\": Команда доступна после авторизации. Показывает все задачи по предикату или имени.\n" +
                              "/exit: Выход из программы.\n");
        }
        /// <summary>
        /// Выполнение команды Info
        /// </summary>
        public void ExecuteInfoCommand(ITelegramBotClient botClient, Update update)
        {
            var name = _currentUser?.TelegramUserName ?? "Пользователь,";
            botClient.SendMessage(update.Message.Chat, name + " Версия ПО: 1.3.9; Дата создания: 10.10.2026г.\n");
        }
        /// <summary>
        /// Выполнение команды Addtask
        /// </summary>
        public void ExecuteAddtaskCommand(ITelegramBotClient botClient, Update update)
        {
            //Проверка пользователя
            if (_currentUser is not null)
            {
                if (_cmd.Length >= 2)
                {
                    var task = _cmd[1];
                    //Проверяем
                    ValidateString(task);

                    //Проверка на установленное количество задач
                    _itemService.isMaxCountTasks(_currentUser);

                    //Проверка на установленную длинну задачи 
                    _itemService.isMaxLenghtTask(task);

                    //Добавляем задачу в список
                    var t = _itemService.Add(_currentUser, task);

                    botClient.SendMessage(update.Message.Chat, "\nЗадача \"" + t.Name + "\" добавлена.\n");
                }
                else
                {
                    botClient.SendMessage(update.Message.Chat, "Вы не ввели название задачи. Повторите снова.");
                }
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вы не авторизовались, выполните сначала команду /start и повторите снова");
            }
        }
        /// <summary>
        /// Выполнение команды Removetask
        /// </summary>
        public void ExecuteRemovetaskCommand(ITelegramBotClient botClient, Update update)
        {
            //Проверка пользователя
            if (_currentUser is not null)
            {
                if (_cmd.Length >= 2)
                {
                    var tasks = _itemService.GetAllByUserId(_currentUser.UserId);
                    //Проверка на наличие задач
                    if (tasks.Any())
                    {
                        if (!Guid.TryParse(_cmd?[1], out Guid id))
                        {
                            botClient.SendMessage(update.Message.Chat, $"Некорректный Id {_cmd?[1]}.");
                        }
                        else
                        {
                            //Удаление задачи с нужным id
                            _itemService.Delete(id);
                            botClient.SendMessage(update.Message.Chat, $"Задача удалена.");
                        }
                    }
                    else
                    {
                        botClient.SendMessage(update.Message.Chat, "Вам нечего удалить, ваш список задач пуст!");
                    }
                }
                else
                {
                    botClient.SendMessage(update.Message.Chat, "Вы не ввели Id задачи. Повторите снова.");
                }
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вы не авторизовались, выполните сначала команду /start и повторите снова");
            }
        }
        /// <summary>
        /// Отображение задач со статусом "Актив"
        /// </summary>
        public void ExecuteShowActiveTasks(ITelegramBotClient botClient, Update update)
        {
            //Проверка пользователя
            if (_currentUser is not null)
            {
                var tasks = _itemService.GetActiveByUserId(_currentUser.UserId);
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
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вы не авторизовались, выполните сначала команду /start и повторите снова");
            }
        }
        /// <summary>
        /// Найти задачу по ID
        /// </summary>
        public void ExecuteCompletetaskCommand(ITelegramBotClient botClient, Update update)
        {
            //Проверка пользователя
            if (_currentUser is not null)
            {
                if (_cmd.Length >= 2)
                {
                    if (!Guid.TryParse(_cmd?[1], out Guid id))
                    {
                        //throw new ArgumentException($"Некорректный Id {_cmd?[1]}.");
                        botClient.SendMessage(update.Message.Chat, $"Некорректный Id {_cmd?[1]}.");
                    }
                    else
                    {
                        //Изменение статуса задачи на "Завершённая"
                        _itemService.MarkCompleted(id);
                        botClient.SendMessage(update.Message.Chat, $"Задаче {_cmd?[1]}, присвоен статус \"Выполненная\".");
                    }
                }
                else
                {
                    botClient.SendMessage(update.Message.Chat, "Вы не ввели Id задачи. Повторите снова.");
                }                
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вы не авторизовались, выполните сначала команду /start и повторите снова");
            }
            
        }
        /// <summary>
        /// Показать все задачи с любым статусом
        /// </summary>
        public void ExecuteShowAllTasksCommand(ITelegramBotClient botClient, Update update)
        {
            //Проверка пользователя
            if (_currentUser is not null)
            {
                var tasks = _itemService.GetAllByUserId(_currentUser.UserId);
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
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вы не авторизовались, выполните сначала команду /start и повторите снова");
            }
            
        }     
        /// <summary>
        /// Показать статистику по задачам
        /// </summary>
        public void ExecuteReportTasksCommand(ITelegramBotClient botClient, Update update)
        {
            //Проверка пользователя
            if (_currentUser is not null)
            {
                var (total, completed, active, generatedAt) = _reportService.GetUserStats(_currentUser.UserId);

                botClient.SendMessage(update.Message.Chat, $"Статистика по задачам на {generatedAt}. Всего: {total}; Завершённых: {completed}; Активныых: {active};");
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вы не авторизовались, выполните сначала команду /start и повторите снова");
            }
        }
        /// <summary>
        /// Показывает все задачи по предикату или имени
        /// </summary>
        public void ExecuteFindTasksCommand(ITelegramBotClient botClient, Update update)
        {
            //Проверка пользователя
            if (_currentUser is not null)
            {
                if (_cmd.Length >= 2)
                {
                    var tasks = _itemService.Find(_currentUser, _cmd[1]);
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
                        botClient.SendMessage(update.Message.Chat, "Ваш список задач пуст! Добавте задачи и повторите снова.");
                    }
                }
                else
                {
                    botClient.SendMessage(update.Message.Chat, "Вы не ввели prefix сортировки задачи. Повторите снова.");
                }
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, "Вы не авторизовались, выполните сначала команду /start и повторите снова");
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
