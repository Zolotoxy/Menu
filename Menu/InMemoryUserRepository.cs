using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private List<ToDoUser> users;
        public void Add(ToDoUser user)
        {
            throw new NotImplementedException();
        }

        public ToDoUser? GetUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public ToDoUser? GetUserByTelegramUserId(long telegramUserId)
        {
            throw new NotImplementedException();
        }
    }
}
