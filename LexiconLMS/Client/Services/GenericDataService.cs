using LexiconLMS.Client.Helpers;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace LexiconLMS.Client.Services;

public interface IGenericDataService
{
    protected const string json = "application/json";
    Task<T?> GetAsync<T>(string path, string contentType = json);
    Task<bool> AddAsync<T>(string path, T objectToAdd);
	Task<bool> UpdateAsync<T>(string path, T objectToUpdate);
	Task<bool> DeleteAsync(string path);
}

public class GenericDataService : IGenericDataService
{
	private readonly HttpClient client;


	public GenericDataService(HttpClient httpClient)
	{
		this.client = httpClient;
		// client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
		//client.Timeout = new TimeSpan(0, 0, 5);
	}

	public async Task<T?> GetAsync<T>(string path, string contentType = IGenericDataService.json)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, path);
		//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

		var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
		response.EnsureSuccessStatusCode();

		var stream = await response.Content.ReadAsStreamAsync();
		var result = JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
		return result;
	}
	

    public async Task<bool> AddAsync<T>(string path, T objectToAdd)
    {

        string json = JsonSerializer.Serialize(objectToAdd);

        var request = new HttpRequestMessage(HttpMethod.Post, path);
        var httpContent = new StringContent(json, new MediaTypeHeaderValue(Application.Json));
        request.Content = httpContent;

        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateAsync<T>(string path, T objectToUpdate)
    {
        try
        {
            string json = JsonSerializer.Serialize(objectToUpdate);

            var request = new HttpRequestMessage(HttpMethod.Put, path);
            var httpContent = new StringContent(json, new MediaTypeHeaderValue(Application.Json));
            request.Content = httpContent;

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex) {
            var ex1 = ex.InnerException;
            return false;
        }
        
    }

    public async Task<bool> DeleteAsync(string path)
    {

        var request = new HttpRequestMessage(HttpMethod.Delete, path);

        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        return false;

    }
}
