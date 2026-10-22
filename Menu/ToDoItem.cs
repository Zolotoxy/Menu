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
        public Guid Id { get; set; }
        public ToDoUser User { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public ToDoItemState State { get; set; }
        public DateTime? StateChangedAt { get; set; }
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
