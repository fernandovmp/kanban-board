using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KanbanBoard.IntegrationTests.WebApi.Helpers
{
    internal static class HttpClientExtensions
    {
        internal static async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
            this HttpClient httpClient,
            string requestUri,
            TValue value
        )
        {
            string json = JsonSerializer.Serialize(value);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(requestUri, httpContent);
        }
    }
}
