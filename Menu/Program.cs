/*
Вам предстоит создать консольное приложение, которое будет имитировать интерактивное меню бота согласно следующему плану:

1. Приветствие: При запуске программы отображается сообщение приветствия со списком доступных команд: /start, /help, /info, /exit.
2. Обработка команды /start: Если пользователь вводит команду /start, программа просит его ввести своё имя. Сохраните введенное имя в переменную.
    Программа должна обращаться к пользователю по имени в каждом следующем ответе.
3. Обработка команды /help: Отображает краткую справочную информацию о том, как пользоваться программой.
4. Обработка команды /info: Предоставляет информацию о версии программы и дате её создания.
5. Доступ к команде /echo: После ввода имени становится доступной команда /echo. При вводе этой команды с аргументом (например, /echo Hello),
    программа возвращает введенный текст (в данном примере "Hello").
6. Основной цикл программы: Программа продолжает ожидать ввод команды от пользователя, пока не будет введена команда /exit.
*/
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Console;

namespace Menu
{
    internal class Program
    {
        //максимальное количество задач
        const int MAX_TASKS = 100;
        //миниимальное количество задач
        const int MIN_TASKS = 1;
        //максимальное количество символов задачи
        const int MAX_LENGHT_TASK = 100;
        //миниимальное количество символов задачи
        const int MIN_LENGHT_TASK = 1;
        //отображение команд без авторизации
        const string NOT_NAME = "Введите пожалуйста команду из списка ниже: \n/start \n/help \n/info \n/exit\n";
        //отображение команд после авторизации
        const string IS_NAME = "введите пожалуйста команду из списка ниже: \n/start \n/help \n/info \n/exit \n/echo\n/addtask\n/showtasks\n/removetask\n";
        static void Main(string[] args)
        {
            //флаг старта программы
            var start = true;
            //кол-во задач введённое пользователем
            var inputCountTasks = 0;
            //введённая длина символов задачи
            var inputLenghtTask = 0;
            //Описание одной задачи
            var task = "";
            //введённый номер задачи
            var numberTask = 0;
            //введённое имя пользователя
            var name = "";
            //Список задач
            var tasks = new List<string>();


            while (start)
            {

                try
                {
                    if (inputCountTasks == 0)
                    {
                        WriteLine("Введите максимальное допустимое количество задач от 1 до 100:");
                        var str = ReadLine();
                        inputCountTasks = ParseAndValidateInt(str, MIN_TASKS, MAX_TASKS);
                    }

                    if (inputLenghtTask == 0)
                    {
                        WriteLine("Введите максимально допустимую длинну символов задачи от 1 до 100:");
                        var str = ReadLine();
                        inputLenghtTask = ParseAndValidateInt(str, MIN_LENGHT_TASK, MAX_LENGHT_TASK);

                        //приветствие
                        WriteLine("Здравтсвуйте! " + NOT_NAME);
                    }

                    var input = ReadLine();
                    //проверяем
                    ValidateString(input);
                    //разбитие строки на команду и аргумент, cmd[0] -команда, cmd[1] -аргумент
                    var cmd = input.Split(" ", 2);

                    //авторизоваться
                    if (cmd[0] == "/start")
                    {
                        ExecuteStartCommand(name, IS_NAME);
                    }
                    //получить инструкцию как пользоваться программой
                    else if (cmd[0] == "/help")
                    {
                        ExecuteHelpCommand(name);
                    }
                    //получить информацию о программе
                    else if (cmd[0] == "/info")
                    {
                        ExecuteInfoCommand(name);
                    }
                    //отображение пользовательского сообщения после ввода команды "/echo"
                    else if (cmd[0] == "/echo")
                    {
                        ExecuteEchoCommand(cmd[1]);
                    }
                    //добавить задачи в список
                    else if (cmd[0] == "/addtask")
                    {
                        ExecuteAddtaskCommand(tasks, task, inputCountTasks, inputLenghtTask, MAX_LENGHT_TASK);
                    }
                    //показать задачи из списка
                    else if (cmd[0] == "/showtasks")
                    {
                        ExecuteShowtasksCommand(tasks);
                    }
                    //удалить задачу из списка
                    else if (cmd[0] == "/removetask")
                    {
                        ExecuteRemovetaskCommand(tasks, numberTask, inputCountTasks, MIN_TASKS);
                    }
                    //выход пользователя из программы
                    else if (cmd[0] == "/exit")
                    {
                        ExecuteExitCommand(ref start);
                    }
                    //если ввёл неправильно команду
                    else
                    {
                        WriteLine("Извините, но такой команды \"" + input + "\" не существует!");
                    }
                }
                catch (TaskCountLimitException ex)
                {
                    WriteLine(ex.Message + ": " + ex.TaskCountLimit);
                }
                catch (TaskLengthLimitException ex)
                {
                    WriteLine(ex.Message + ": " + ex.TaskLenghtLimit);
                }
                catch (DublicateTaskException ex)
                {
                    WriteLine(ex.Message + ": " + ex.Task);
                }
                catch (ArgumentException ex)
                {
                    WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message + ' ' + ex.GetType());
                    WriteLine(ex.Message + ' ' + ex.StackTrace);
                    WriteLine(ex.Message + ' ' + ex.InnerException);
                    //throw;
                }
            }
        }
        /// <summary>
        /// Приводим строку к int и проверяет на нахождение в диапазон и выдаём ошибку ели не проходит
        /// </summary>
        public static int ParseAndValidateInt(string? str, int min, int max)
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
        public static void ValidateString(string? str)
        {
            str = (str ?? string.Empty).Trim();
            if (String.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Некорректно введёно поле, пожалуйста попробуйте ещё раз.");
            }
        }
        /// <summary>
        /// Выполнение команды Start
        /// </summary>
        public static void ExecuteStartCommand(string? name, string IS_NAME)
        {
            WriteLine("Введите ваше имя:");
            //ввод имени
            name = ReadLine();
            //Проверяем
            ValidateString(name);

            WriteLine("\nДобро пожаловать " + name + "!\nУважаемый " + name + ", " + IS_NAME);
        }
        /// <summary>
        /// Выполнение команды Help
        /// </summary>
        public static void ExecuteHelpCommand(string name)
        {
            WriteLine(name + " Вот список команд:\n" +
                              "/start: Авторизация, ввод имени пользователя.\n" +
                              "/help: Отображает краткую справочную информацию о том, как пользоваться программой.\n" +
                              "/info: Предоставляет информацию о версии программы и дате её создания.\n" +
                              "/echo: Команда доступна после авторизации. При вводе этой команды с аргументом \"/echo Hello\" программа возвращает текст \"Hello\")\n" +
                              "/addtask: Команда доступна после авторизации. Добавление задачи в список.\n" +
                              "/showtasks: Команда доступна после авторизации. Показать задачи в списоке.\n" +
                              "/removetask: Команда доступна после авторизации. Удалить задачу в списоке.\n" +
                              "/exit: Выход из программы.\n");
        }
        /// <summary>
        /// Выполнение команды Info
        /// </summary>
        public static void ExecuteInfoCommand(string name)
        {
            WriteLine(name + " Версия ПО: 1.1.0; Дата создания: 10.10.2026г.\n");
        }
        /// <summary>
        /// Выполнение команды Echo
        /// </summary>
        public static void ExecuteEchoCommand(string cmd)
        {
            //ввывод аргументов после введённой команды
            WriteLine(cmd);
        }
        /// <summary>
        /// Выполнение команды Addtask
        /// </summary>
        public static void ExecuteAddtaskCommand(List<string> tasks, string task, int inputCountTasks, int inputLenghtTask, int MAX_LENGHT_TASK)
        {
            WriteLine("Пожалуйста, введите описание задачи:");
            task = ReadLine();
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
            if (tasks.Contains(task))
            {
                throw new DublicateTaskException($"Задача {task} уже существует", task);
            }

            //добавляем задачу в список
            tasks.Add(task);
            WriteLine("\nЗадача \"" + task + "\" добавлена.\n");
        }
        /// <summary>
        /// Выполнение команды Showtasks
        /// </summary>
        public static void ExecuteShowtasksCommand(List<string> tasks)
        {
            if (tasks.Count > 0)
            {
                var j = 0;
                //вывод циклом задач из списка
                for (int i = 0; i < tasks.Count; i++)
                {
                    WriteLine($"{++j}. {tasks[i]}");
                }
            }
            else
            {
                WriteLine("Ваш список задач пуст! Добавте задачи.");
            }
        }
        /// <summary>
        /// Выполнение команды Removetask
        /// </summary>
        public static void ExecuteRemovetaskCommand(List<string> tasks, int numberTask, int inputCountTasks, int MIN_TASKS)
        {
            //Проверка на наличие задач
            if (tasks.Count >= MIN_TASKS)
            {
                WriteLine("Вот ваш список задач:\n");
                var j = 0;
                //вывод циклом задач из списка
                for (int i = 0; i < tasks.Count; i++)
                {
                    WriteLine($"{++j}. {tasks[i]}");
                }

                WriteLine("Введите номер задачи для удаления:\n");
                var str = ReadLine();
                numberTask = ParseAndValidateInt(str, 0, inputCountTasks);
                //удаляем задачу из списка
                WriteLine($"Задача {tasks[numberTask -= 1]} удалена.");
                tasks.RemoveAt(numberTask);
            }
            else
            {
                WriteLine("Вам нечего удалить, ваш список задач пуст!");
            }
        }
        /// <summary>
        /// Выполнение команды Exit
        /// </summary>
        public static void ExecuteExitCommand(ref bool start)
        {
            start = false;
        }
    }
}
