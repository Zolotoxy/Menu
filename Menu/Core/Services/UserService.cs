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
        public async Task<ToDoUser?> GetUser(long telegramUserId, CancellationToken ct)
        {
            var user = await _repository.GetUserByTelegramUserId(telegramUserId, ct);
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
        public async Task<ToDoUser?> GetUser(Guid userId, CancellationToken ct)
        {
            var user = await _repository.GetUser(userId, ct);
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
        public async Task<ToDoUser> RegisterUser(long telegramUserId, string telegramUserName, CancellationToken ct)
        {
            var existing = await _repository.GetUserByTelegramUserId(telegramUserId, ct);
            if (existing is not null)
            {
                return existing;
            }
            else
            {
                var user = new ToDoUser(telegramUserName, telegramUserId);
                _repository.Add(user, ct);
                return user;
            }
        }















    }
}
