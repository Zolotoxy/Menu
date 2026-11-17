using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Exceptions
{
    public class TaskLengthLimitException : Exception
    {
        public int TaskLenght { get; }
        public int TaskLenghtLimit { get; }
        public TaskLengthLimitException(string message, int taskLenght, int taskLenghtLimit) : base(message)
        {
            this.TaskLenght = taskLenght;
            this.TaskLenghtLimit = taskLenghtLimit;
        }
    }
}
