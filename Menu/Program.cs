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

namespace Menu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //флаг старта программы
            var start = true;
            //максимальное количество задач
            const int MAX_TASKS = 100;
            //миниимальное количество задач
            const int MIN_TASKS = 1;
            //максимальное количество символов задачи
            const int MAX_LENGHT_TASK = 100;
            //миниимальное количество символов задачи
            const int MIN_LENGHT_TASK = 1;
            //кол-во задач введённое пользователем
            var inputCountTasks = 1;
            //введённая длина символов задачи
            var inputLenghtTask = 1;
            //разрешение проверки на кол-во задач
            var isCount = true;
            //разрешение проверки на кол-во символов
            var isLenght = true;

            while (true)
            {
                try
                {
                    if (isCount)
                    {
                        Console.WriteLine("Введите максимальное допустимое количество задач от 1 до 100:");
                        var str = Console.ReadLine();
                        inputCountTasks = ParseAndValidateInt(str, MIN_TASKS, MAX_TASKS);
                        isCount = false;
                    }

                    if (isLenght)
                    {
                        Console.WriteLine("Введите максимально допустимую длинну символов задачи от 1 до 100:");
                        var str = Console.ReadLine();
                        inputLenghtTask = ParseAndValidateInt(str, MIN_LENGHT_TASK, MAX_LENGHT_TASK);
                        isLenght = false;
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            //Описание одной задачи
            var task = "";
            //введённый номер задачи
            var numberTask = 0;
            //Список задач
            var tasks = new List<string>();
            //введённое имя пользователя
            var name = "";
            //отображение команд без авторизации
            var notName = "Введите пожалуйста команду из списка ниже: \n/start \n/help \n/info \n/exit\n";
            //отображение команд после авторизации
            var isName = "введите пожалуйста команду из списка ниже: \n/start \n/help \n/info \n/exit \n/echo\n/addtask\n/showtasks\n/removetasks\n";
            //приветствие
            Console.WriteLine("Здравтсвуйте! " + notName);

            //основной цикл выполнения программы
            while (start)
            {
                try
                {
                    var input = Console.ReadLine();
                    //проверяем
                    ValidateString(input);
                    //разбитие строки на команду и аргумент, cmd[0] -команда, cmd[1] -аргумент
                    var cmd = input.Split(" ", 2);

                    //авторизоваться
                    if (cmd[0] == "/start")
                    {
                        Console.WriteLine("Введите ваше имя:");
                        //ввод имени
                        name = Console.ReadLine();
                        //Проверяем
                        ValidateString(name);

                        Console.WriteLine("\nДобро пожаловать " + name + "!\nУважаемый " + name + ", " + isName);
                    }
                    //получить инструкцию как пользоваться программой
                    else if (cmd[0] == "/help")
                    {
                        Console.WriteLine(name + " Вот список команд:\n" +
                              "/start: Авторизация, ввод имени пользователя.\n" +
                              "/help: Отображает краткую справочную информацию о том, как пользоваться программой.\n" +
                              "/info: Предоставляет информацию о версии программы и дате её создания.\n" +
                              "/echo: Команда доступна после авторизации. При вводе этой команды с аргументом \"/echo Hello\" программа возвращает текст \"Hello\")\n" +
                              "/addtask: Команда доступна после авторизации. Добавление задачи в список.\n" +
                              "/showtasks: Команда доступна после авторизации. Показать задачи в списоке.\n" +
                              "/removetask: Команда доступна после авторизации. Удалить задачу в списоке.\n" +
                              "/exit: Выход из программы.\n");
                    }
                    //получить информацию о программе
                    else if (cmd[0] == "/info")
                    {
                        Console.WriteLine(name + " Версия ПО: 1.0.5; Дата создания: 10.10.2026г.\n");
                    }
                    //отображение пользовательского сообщения после ввода команды "/echo"
                    else if (cmd[0] == "/echo")
                    {
                        //ввывод аргументов после введённой команды
                        Console.WriteLine(cmd[1]);
                    }
                    //добавить задачи в список
                    else if (cmd[0] == "/addtask")
                    {
                        Console.WriteLine("Пожалуйста, введите описание задачи:");
                        task = Console.ReadLine();
                        //Проверяем
                        ValidateString(task);
                        //Проверка на установленное количество задач
                        if (tasks.Count >= inputCountTasks)
                        {
                            throw new TaskCountLimitException($"Превышено максимальное количество задач равное: {inputCountTasks}", inputCountTasks);
                        }
                        else
                        {
                            //Проверка на установленную длинну задачи 
                            if (task.Length >= inputLenghtTask)
                            {
                                throw new TaskLengthLimitException($"Длинна задачи {inputLenghtTask} превышает максимально допустимое значение {MAX_LENGHT_TASK}", inputLenghtTask, MAX_LENGHT_TASK);
                            }
                            else
                            {
                                //Проверка на дубликаты задач
                                if (tasks.Contains(task))
                                {
                                    throw new DublicateTaskException($"Задача {task} уже существует", task);

                                }
                                else
                                {
                                    tasks.Add(task);
                                    //добавляем задачу в список
                                    Console.WriteLine("\nЗадача \"" + task + "\" добавлена.\n");
                                }
                            }
                        }
                    }
                    //показать задачи из списка
                    else if (cmd[0] == "/showtasks")
                    {
                        if (tasks.Count > 0)
                        {
                            var j = 0;
                            //вывод циклом задач из списка
                            for (int i = 0; i < tasks.Count; i++)
                            {
                                Console.WriteLine($"{++j}. {tasks[i]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ваш список задач пуст! Добавте задачи.");
                        }

                    }
                    //удалить задачу из списка
                    else if (cmd[0] == "/removetask")
                    {
                        //Проверка на наличие задач
                        if (tasks.Count >= MIN_TASKS)
                        {
                            Console.WriteLine("Вот ваш список задач:\n");
                            var j = 0;
                            //вывод циклом задач из списка
                            for (int i = 0; i < tasks.Count; i++)
                            {
                                Console.WriteLine($"{++j}. {tasks[i]}");
                            }

                            Console.WriteLine("Введите номер задачи для удаления:\n");
                            var str = Console.ReadLine();
                            numberTask = ParseAndValidateInt(str, 0, inputCountTasks);
                            //удаляем задачу из списка
                            Console.WriteLine($"Задача {tasks[numberTask -= 1]} удалена.");
                            tasks.RemoveAt(numberTask);
                        }
                        else
                        {
                            Console.WriteLine("Вам нечего удалить, ваш список задач пуст!");
                        }
                    }
                    //выход пользователя из программы
                    else if (cmd[0] == "/exit")
                    {
                        start = false;
                    }
                    //если ввёл неправильно команду
                    else
                    {
                        Console.WriteLine("Извините, но такой команды \"" + input + "\" не существует!");
                    }
                }
                catch (TaskCountLimitException ex)
                {
                    Console.WriteLine(ex.Message + ": " + ex.TaskCountLimit);
                }
                catch (TaskLengthLimitException ex)
                {
                    Console.WriteLine(ex.Message + ": " + ex.TaskLenghtLimit);
                }
                catch (DublicateTaskException ex)
                {
                    Console.WriteLine(ex.Message + ": " + ex.Task);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ' ' + ex.GetType());
                    Console.WriteLine(ex.Message + ' ' + ex.StackTrace);
                    Console.WriteLine(ex.Message + ' ' + ex.InnerException);
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
    }
}
