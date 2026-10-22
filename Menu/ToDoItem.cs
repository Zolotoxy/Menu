using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    /// <summary>
    /// Регистрация задачи
    /// </summary>
    internal class ToDoItem
    {
        public Guid Id;
        public ToDoUser User;
        public string Name;
        public DateTime CreatedAt;
        public ToDoItemState State;
        public DateTime? StateChangedAt;
        public ToDoItem(ToDoUser user, string name)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            State = ToDoItemState.Active;

            User = user;
            Name = name;
        }
    }
}
