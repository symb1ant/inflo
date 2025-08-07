using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace UserManagement.UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");


        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? builder.HostEnvironment.BaseAddress) });


        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? builder.HostEnvironment.BaseAddress);
        });

        builder.Services.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNameCaseInsensitive = true;
        });


        await builder.Build().RunAsync();
    }
}
