using Menu.Core.DataAccess;
using Menu.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    /// <summary>
    /// Работа с хранилицем пользователей
    /// </summary>
    internal class UserService : IUserService
    {
        //Список пользователей
        //private readonly Dictionary<long, ToDoUser> _users = new Dictionary<long, ToDoUser>();
        IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        //поиск пользователя по telegramUserId
        public ToDoUser? GetUser(long telegramUserId)
        {
            var user = _repository.GetUserByTelegramUserId(telegramUserId);
            if (user is not null)
            {
                return user;
            }
            else
            {
                throw new Exception($"Пользователя с \"{telegramUserId}\" не существует.");
            }
        }
        //поиск пользователя по userId
        public ToDoUser? GetUser(Guid userId)
        {
            var user = _repository.GetUser(userId);
            if (user is not null)
            {
                return user;
            }
            else
            {
                throw new Exception($"Пользователя с \"{userId}\" не существует.");
            }
        }

        //Добавления пользователя в список и проверка на существующего
        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            var existing = _repository.GetUserByTelegramUserId(telegramUserId);
            if (existing is not null)
            {
                return existing;
            }
            else
            {
                var user = new ToDoUser(telegramUserName, telegramUserId);
                _repository.Add(user);
                return user;
            }
        }















    }
}
