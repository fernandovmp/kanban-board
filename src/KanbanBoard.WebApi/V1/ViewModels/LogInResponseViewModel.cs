namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class LogInResponseViewModel
    {
        public string Token { get; set; }
        public UserViewModel User { get; set; }
    }
}
