using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    /// <summary>
    /// Хранилище пользователей
    /// </summary>
    internal class UserService : IUserService
    {
        //Список пользователей
        private readonly Dictionary<long, ToDoUser> _users = new Dictionary<long, ToDoUser>();
        //Поиск пользователя
        public ToDoUser? GetUser(long telegramUserId)
        {
            return _users.TryGetValue(telegramUserId, out var user) ? user : null;
        }
        //Добавления пользователя в список и проверка на существующего
        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            if (_users.TryGetValue(telegramUserId, out var existing))
            {
                return existing;
            }
            var user = new ToDoUser(telegramUserName, telegramUserId);
            _users[telegramUserId] = user;

            return user;
        }
    }
}
