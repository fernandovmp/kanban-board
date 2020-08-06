namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public int Status { get; }
        public string Message { get; }
    }
}
