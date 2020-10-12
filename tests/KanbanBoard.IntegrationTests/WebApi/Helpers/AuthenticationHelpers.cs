using System.Net.Http;
using System.Threading.Tasks;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.IntegrationTests.WebApi.Helpers
{
    internal class AuthenticationData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    internal static class AuthenticationHelpers
    {
        internal static async Task<string> GetToken(TestFixture fixture, AuthenticationData user)
        {
            using HttpClient client = fixture.Factory.CreateClient();
            var userData = new SignUpViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                ConfirmPassword = user.Password
            };
            var loginData = new LogInViewModel
            {
                Email = user.Email,
                Password = user.Password
            };
            _ = await client.PostAsJsonAsync(requestUri: "api/v1/users", userData);
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/v1/login", loginData);
            LogInResponseViewModel loginResponseData = await response.DeserializeAsAsync<LogInResponseViewModel>();
            return loginResponseData.Token;
        }
    }
}
