using LexiconLMS.Client.Components;
using LexiconLMS.Shared.Entities;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Net.Http.Headers;

namespace LexiconLMS.Client.Services
{
    public class ActivityDataService : IActivityDataService
    {
        HttpClient _httpClient;
        MediaTypeHeaderValue _mediaTypeHeaderValue;

        public ActivityDataService()
        {
            _httpClient = new HttpClient();
            _mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
        }
          public async Task<bool> AddActivity(Activity activity)
        {
            var json = JsonSerializer.Serialize(activity);
            var httpContent = new StringContent(json, _mediaTypeHeaderValue);
            var response = await _httpClient.PostAsync("/activity/add", httpContent);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        public void DeleteActivity(Guid Id)
        {
           

         
        }

        public async Task<Activity> GetActivitiy(Guid Id)
        {
            var json = JsonSerializer.Serialize(Id);
            var httpContent = new StringContent(json, _mediaTypeHeaderValue);
            var response = await _httpClient.PostAsync("/activity/getactivity/", httpContent);

            string apiUrl = $"/activity/{Id}";
            var response1 = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

                var result = ta
                return (Activity)JsonSerializer.Deserialize<Activity>(responseData, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Activity>> GetActivities()
        {

            var response = await _httpClient.GetAsync("/activities");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

                return (List<Activity>)JsonSerializer.Deserialize<IEnumerable<Activity>>(responseData, options);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateActivity (Activity updatedActivity)
        {
            var activity = GetActivitiy(updatedActivity.Id).Result;
            if (activity != null)
            {
                activity.Name = updatedActivity.Name;
                activity.StartDate = updatedActivity.StartDate;
                activity.EndDate = updatedActivity.EndDate;
                activity.Description = updatedActivity.Description;

                var json = JsonSerializer.Serialize(activity);
                var httpContent = new StringContent(json, _mediaTypeHeaderValue);
                var response = await _httpClient.PostAsync("/activity/update", httpContent);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
