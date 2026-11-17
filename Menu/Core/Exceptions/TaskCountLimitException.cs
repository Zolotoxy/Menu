using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Exceptions
{
    public class TaskCountLimitException : ArgumentException
    {
        public int TaskCountLimit { get; }
        public TaskCountLimitException(string message, int taskCountLimit) : base (message)
        {
            this.TaskCountLimit = taskCountLimit;
        }
    }
}
