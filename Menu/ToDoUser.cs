using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    internal class ToDoUser
    {
        public Guid UserId { get; }
        public string TelegramUserName {  get; }
        public DateTime RegisteredAt { get; }
        public ToDoUser(string telegramUserName)
        {
            UserId = Guid.NewGuid();
            TelegramUserName = telegramUserName;
            RegisteredAt = DateTime.UtcNow;
        }
    }
}
