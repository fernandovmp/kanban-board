using System;

namespace KanbanBoard.WebApi.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now();
        DateTime UtcNow();
    }
}
