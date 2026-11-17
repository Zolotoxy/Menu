using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Console;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using Menu.Infrastructure.DataAccess;
using Menu.Core.Services;
using Menu.TelegramBot;

namespace Menu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var userRepositiry = new InMemoryUserRepository();
                var itemRepository = new InMemoryToDoRepository();

                var userService = new UserService(userRepositiry);
                var itemService = new ToDoService(itemRepository);
                var reportService = new ToDoReportService(itemRepository);

                var handler = new UpdateHandler(userService, itemService, reportService);

                var botClient = new ConsoleBotClient();

                botClient.StartReceiving(handler);
            }

            catch (Exception ex)
            {
                WriteLine($"{ex.Message}; {ex?.InnerException?.Message}");
            }
        }
    }
}
