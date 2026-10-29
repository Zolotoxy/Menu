using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    /// <summary>
    /// Интерфейс хранилища пользователей
    /// </summary>
    internal interface IUserService
    {
        //регистрация
        ToDoUser RegisterUser(long telegramUserId, string telegramUserName);
        //поиск
        ToDoUser? GetUser(long telegramUserId);
    }
}
