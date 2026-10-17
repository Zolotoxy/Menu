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
using static System.Net.Mime.MediaTypeNames;

namespace Menu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //флаг старта программы
            var start = true;
            //Список задач
            var tasks = new List<string>();
            //Описание одной задачи
            var task = "";
            //введённый номер задачи
            var numberTask = 0;
            //введённое имя пользователя
            var name = "";
            //отображение команд без авторизации
            var notName = "Введите пожалуйста команду из списка ниже: \n/start \n/help \n/info \n/exit\n";
            //отображение команд после авторизации
            var isName = "введите пожалуйста команду из списка ниже: \n/start \n/help \n/info \n/exit \n/echo\n/addtask\n/showtasks\n/removetasks\n";
            //приветствие
            Console.WriteLine("Здравтсвуйте! " + notName);


            while (start)
            {
                try
                {
                    //ввод команды, убираем пробелы
                    var input = (Console.ReadLine() ?? string.Empty).Trim();
                    //разбитие строки на команду и аргумент, cmd[0] -команда, cmd[1] -аргумент
                    var cmd = input.Split(" ", 2);

                    //авторизоваться
                    if (cmd[0] == "/start")
                    {
                        Console.WriteLine("Введите ваше имя:");
                        //ввод имени, убираем пробелы
                        name = (Console.ReadLine() ?? string.Empty).Trim();
                        //проверка пользователя на пустое имя
                        Console.WriteLine(String.IsNullOrEmpty(name) ? "Некорректно введёно имя, пожалуйста попробуйте ещё раз." : "\nДобро пожаловать " + name + "!\n");
                        //проверка пользователя на пустое имя и отображение команд после авторизации
                        Console.WriteLine(String.IsNullOrEmpty(name) ? notName : "Уважаемый " + name + ", " + isName);
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
                        //ввод названия задачи, убираем пробелы
                        task = (Console.ReadLine() ?? string.Empty).Trim();
                        //проверка на пустое название задачи
                        if (String.IsNullOrEmpty(task))
                        {
                            Console.WriteLine("Некорректно введёно название задачи, пожалуйста попробуйте ещё раз.");
                        }
                        else
                        {
                            //добавляем задачу в список
                            Console.WriteLine("\nЗадача \"" + task + "\" добавлена.\n");
                            tasks.Add(task);
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
                        if (tasks.Count > 0)
                        {
                            Console.WriteLine("Вот ваш список задач:\n");
                            var j = 0;
                            //вывод циклом задач из списка
                            for (int i = 0; i < tasks.Count; i++)
                            {
                                Console.WriteLine($"{++j}. {tasks[i]}");
                            }

                            Console.WriteLine("Введите номер задачи для удаления:\n");
                            //переводим введёную задачу в int и проверяем на диапазон кол-во задач
                            if (int.TryParse(Console.ReadLine(), out numberTask) && (numberTask > 0 && numberTask <= tasks.Count))
                            {
                                //удаляем задачу из списка
                                Console.WriteLine($"Задача {tasks[numberTask = -1]} удалена.");
                                tasks.RemoveAt(numberTask);
                            }
                            else
                            {
                                Console.WriteLine("Вы ввели некорректный номер задачи для удаления \" " + numberTask + " \", ");
                            }
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ' ' + ex.GetType());
                    Console.WriteLine(ex.Message + ' ' + ex.StackTrace);
                    Console.WriteLine(ex.Message + ' ' + ex.InnerException);
                    //throw;
                }
            }
        }
    }
}
