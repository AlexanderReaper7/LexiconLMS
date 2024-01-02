using LexiconLMS.Client.Components;
using LexiconLMS.Shared.Entities;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using LexiconLMS.Client.Helpers;
using System.Reflection;

namespace LexiconLMS.Client.Services
{
    public class ActivityDataService : IActivityDataService
    {
        private readonly HttpClient http;
        MediaTypeHeaderValue _mediaTypeHeaderValue;

        public ActivityDataService(HttpClient httpClient)
        {
            http = httpClient;
            _mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
        }
          public async Task<bool> AddActivity(Activity activity)
        {
            var json = JsonSerializer.Serialize(activity);
            var httpContent = new StringContent(json, _mediaTypeHeaderValue);
            var response = await http.PostAsync("/activityadd", httpContent);
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
            HttpResponseMessage response = await http.GetAsync(UriHelper.GetActivityUri(Id.ToString()));

            //string apiUrl = $"/activity/{Id}";
            //var response1 = await http.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

                if (!string.IsNullOrEmpty(responseData))
                {
                    return JsonSerializer.Deserialize<Activity>(responseData, options);
                }
            }          
            return null;
            
        }

        public async Task<List<Activity>> GetActivities()
        {

            var response = await http.GetAsync("/activities");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

              //  return (List<Activity>)JsonSerializer.Deserialize<IEnumerable<Activity>>(responseData, options);
                return JsonSerializer.Deserialize<List<Activity>>(responseData, options)!;
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
                var response = await http.PostAsync("/activity/update", httpContent);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
