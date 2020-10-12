using System;

namespace KanbanBoard.WebApi.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.Now;
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
