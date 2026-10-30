using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    internal class ToDoReportService : IToDoReportService
    {
        public (int total, int completed, DateTime generatedAt) GetUserStats(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
