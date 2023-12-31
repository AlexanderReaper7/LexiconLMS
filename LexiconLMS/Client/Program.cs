using LexiconLMS.Client;
using LexiconLMS.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace LexiconLMS.Client;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient("LexiconLMS.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        // Supply HttpClient instances that include access tokens when making requests to the server project
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("LexiconLMS.ServerAPI"));

        builder.Services.AddApiAuthorization();


        builder.Services.AddScoped<ICourseDataService, CourseDataService>();
        builder.Services.AddScoped<IGenericDataService, GenericDataService>();
        builder.Services.AddScoped<IActivityDataService, ActivityDataService>();
        builder.Services.AddScoped<IModuleDataService, ModuleDataService>();
        builder.Services.AddScoped<IApplicationUserDataService, ApplicationUserDataService>(); 

        await builder.Build().RunAsync();
    }
}
