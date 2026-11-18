using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    internal interface IToDoReportService
    {
        //Возвращает статус всех задач пользователя
        Task<(int total, int completed, int active, DateTime generatedAt)> GetUserStats(Guid userId, CancellationToken ct);
    }
}
