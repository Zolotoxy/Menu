using Menu.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    internal class ToDoReportService : IToDoReportService
    {
        private IToDoRepository _toDoRepository;
        //токен
        private CancellationToken _ct;

        public ToDoReportService(IToDoRepository toDoRepository, CancellationToken ct)
        {
            _toDoRepository = toDoRepository;
            _ct = ct;
        }
        //Статистика по задачам пользователя
        public async Task<(int total, int completed, int active, DateTime generatedAt)> GetUserStats(Guid userId, CancellationToken ct)
        {
            int total = 0, active =0, completed = 0;
            DateTime generatedAt;

            //Получение всех задач пользователя
            var tasksUser = await _toDoRepository.GetAllByUserId(userId, _ct);
            foreach (var t in tasksUser)
            {
                if (t.State == Entities.ToDoItemState.Completed)
                {
                    completed ++;
                }
                if (t.State == Entities.ToDoItemState.Active)
                {
                    active++;
                }
            }
            total = tasksUser.Count;
            generatedAt = DateTime.UtcNow;

            return (total, completed, active, generatedAt);
        }
    }
}
