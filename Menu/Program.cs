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
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace Menu
{

    internal class Program
    {
        
        static void Main(string[] args)
        {
            

            var handler = new UpdateHandler();
            var botClient = new ConsoleBotClient();
            botClient.StartReceiving(handler);
            var update = new Update();


            //while (true)
            //{
            //    botClient.StartReceiving(handler);


            //    try
            //    {
            //        //handler.HandleUpdateAsync(botClient, update);
            //    }

            //    catch (Exception ex)
            //    {
            //        WriteLine(ex.Message + ' ' + ex.GetType());
            //        WriteLine(ex.Message + ' ' + ex.StackTrace);
            //        WriteLine($"{ex.Message}; {ex?.InnerException?.Message}");
            //        //throw;
            //    }
            //}
        }
    }
}
