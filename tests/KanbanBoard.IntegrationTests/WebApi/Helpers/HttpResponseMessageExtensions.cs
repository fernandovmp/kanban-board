using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace KanbanBoard.IntegrationTests.WebApi.Helpers
{
    internal static class HttpResponseMessageExtensions
    {
        internal static async Task<T> DeserializeAsAsync<T>(
            this HttpResponseMessage httpResponse
        )
        {
            string jsonContent = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
