using Menu.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    /// <summary>
    /// Интерфейс хранилища пользователей
    /// </summary>
    internal interface IUserService
    {
        //регистрация пользователя
        Task<ToDoUser> RegisterUser(long telegramUserId, string telegramUserName, CancellationToken ct);
        //поиск пользователя по telegramUserId
        Task<ToDoUser?> GetUser(long telegramUserId, CancellationToken ct);
        //поиск пользователя по userId
        Task<ToDoUser?> GetUser(Guid userId, CancellationToken ct);
    }
}
