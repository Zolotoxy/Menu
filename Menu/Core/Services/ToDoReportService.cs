using Menu.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menu.Core.Services
{
    internal class ToDoReportService : IToDoReportService
    {
        private IToDoRepository _toDoRepository;

        public ToDoReportService(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }
        //Статистика по задачам пользователя
        public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
        {
            int total = 0, active =0, completed = 0;
            DateTime generatedAt;

            //Получение всех задач пользователя
            var tasksUser = _toDoRepository.GetAllByUserId(userId);
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
