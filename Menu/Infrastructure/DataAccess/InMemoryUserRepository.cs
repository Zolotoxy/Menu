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
        public async Task Add(ToDoUser user, CancellationToken ct)
        {
            if (_users.FirstOrDefault(x => x.TelegramUserId == user.TelegramUserId) is not null)
            {
                throw new ArgumentException($"Пользователь с TelegramUserId = {user.TelegramUserId} уже существует.");
            }
            
            _users.Add(user);
        }
        //Поиск и возврат пользователя по userId
        public Task<ToDoUser?> GetUser(Guid userId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var user = _users.FirstOrDefault(x => x.UserId == userId);
            return Task.FromResult(user);
        }
        //Поиск и возврат пользователя по telegramUserId
        public Task<ToDoUser?> GetUserByTelegramUserId(long telegramUserId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var user = _users.FirstOrDefault(x => x.TelegramUserId == telegramUserId);
            return Task.FromResult(user);
        }
    }
}
