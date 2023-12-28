using System.Net.Http.Headers;
using System.Text.Json;

namespace LexiconLMS.Client.Services;

public interface IGenericDataService
{
    protected const string json = "application/json";
    Task<T?> GetAsync<T>(string path, string contentType = json);
}

public class GenericDataService : IGenericDataService
{
    private readonly HttpClient client;


    public GenericDataService(HttpClient httpClient)
    {
        this.client = httpClient;
        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
        client.Timeout = new TimeSpan(0, 0, 5);
    }

    public async Task<T?> GetAsync<T>(string path, string contentType = IGenericDataService.json)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var result = JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return result;

    }
}
