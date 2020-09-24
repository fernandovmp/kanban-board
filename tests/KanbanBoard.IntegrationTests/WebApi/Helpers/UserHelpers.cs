using System.Net.Http;
using System.Threading.Tasks;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.IntegrationTests.WebApi.Helpers
{
    internal static class UserHelpers
    {
        internal static async Task<UserViewModel> CreateUser(TestFixture fixture, SignUpViewModel user)
        {
            using HttpClient client = fixture.Factory.CreateClient();
            HttpResponseMessage postResponse = await client.PostAsJsonAsync(requestUri: "api/v1/users", user);
            return await postResponse.DeserializeAsAsync<UserViewModel>();
        }
    }
}
