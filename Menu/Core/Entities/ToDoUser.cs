using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Entities
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    internal class ToDoUser
    {
        public Guid UserId { get; set; }
        public string TelegramUserName {  get; set; }
        public DateTime RegisteredAt { get; set; }
        public long TelegramUserId {  get; set; }
        public ToDoUser(string telegramUserName, long telegramUserId)
        {
            UserId = Guid.NewGuid();
            TelegramUserName = telegramUserName;
            RegisteredAt = DateTime.UtcNow;
            TelegramUserId = telegramUserId;
        }
    }
}
