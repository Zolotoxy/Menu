using Menu.Core.DataAccess;
using Menu.Core.Entities;
using Otus.ToDoList.ConsoleBot.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Infrastructure.DataAccess
{
    /// <summary>
    /// Работа с пользователями
    /// </summary>
    internal class InMemoryUserRepository : IUserRepository
    {
        private readonly List<ToDoUser> _users = new List<ToDoUser>();
        //Добавление пользователя в список
        public void Add(ToDoUser user)
        {
            if (_users.FirstOrDefault(x => x.TelegramUserId == user.TelegramUserId) is not null)
            {
                throw new ArgumentException($"Пользователь с TelegramUserId = {user.TelegramUserId} уже существует.");
            }
            
            _users.Add(user);
        }
        //Поиск и возврат пользователя по userId
        public ToDoUser? GetUser(Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.UserId == userId);
            return user;
        }
        //Поиск и возврат пользователя по telegramUserId
        public ToDoUser? GetUserByTelegramUserId(long telegramUserId)
        {
            var user = _users.FirstOrDefault(x => x.TelegramUserId == telegramUserId);
            return user;
        }
    }
}
