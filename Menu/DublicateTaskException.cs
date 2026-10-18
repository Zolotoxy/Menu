using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Menu
{
    public class DublicateTaskException : Exception
    {
        public string Task { get; }
        public DublicateTaskException(string message, string task) : base(message)
        {
            this.Task = task;
        }
    }
}
