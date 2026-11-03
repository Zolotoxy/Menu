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
        ToDoUser? GetUser(Guid userId);
        //Поиск и возврат пользователя по telegramUserId
        ToDoUser? GetUserByTelegramUserId(long telegramUserId);
        //Добавление пользователя в список
        void Add(ToDoUser user);
    }
}
