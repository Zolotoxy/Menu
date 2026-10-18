using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    public class TaskLengthLimitException : Exception
    {
        public int TaskLenghtLimit { get; }
        public TaskLengthLimitException(string message, int taskLenghtLimit) : base(message)
        {
            this.TaskLenghtLimit = taskLenghtLimit;
        }
    }
}
