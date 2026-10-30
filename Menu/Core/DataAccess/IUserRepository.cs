using Menu.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.DataAccess
{
    internal interface IUserRepository
    {
        ToDoUser? GetUser(Guid userId);
        ToDoUser? GetUserByTelegramUserId(long telegramUserId);
        void Add(ToDoUser user);
    }
}
