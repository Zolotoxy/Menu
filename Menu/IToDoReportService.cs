using System;
using System.Collections.Generic;
using System.Text;

namespace Menu
{
    internal interface IToDoReportService
    {
        //Возвращает статус всех задач пользователя
        (int total, int completed, DateTime generatedAt) GetUserStats(Guid userId);
    }
}
