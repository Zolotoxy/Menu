using Menu.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.DataAccess
{
    //Интерфейс работы с хранилицем
    internal interface IUserRepository
    {
        //Поиск и возврат пользователя по userId
        Task<ToDoUser>? GetUser(Guid userId, CancellationToken ct);
        //Поиск и возврат пользователя по telegramUserId
        Task<ToDoUser>? GetUserByTelegramUserId(long telegramUserId, CancellationToken ct);
        //Добавление пользователя в список
        Task Add(ToDoUser user, CancellationToken ct);
    }
}
