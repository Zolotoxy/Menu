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
        ToDoUser RegisterUser(long telegramUserId, string telegramUserName);
        //поиск пользователя по telegramUserId
        ToDoUser? GetUser(long telegramUserId);
        //поиск пользователя по userId
        ToDoUser? GetUser(Guid userId);
    }
}
