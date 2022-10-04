using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

using AdaptiveCards.Blazor;
using AdaptiveCards.Blazor.ActionHandlers;
using AdaptiveCards.Blazor.Actions;
using AdaptiveCards.Rendering.Html;

namespace Stories.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<AdaptiveOpenUrlActionAdapter>();
            builder.Services.AddSingleton<AdaptiveCardRenderer>();
            builder.Services.AddSingleton<ISubmitActionHandler, DefaultSubmitActionHandler>();
            builder.Services.AddSingleton<BlazorAdaptiveCardsOptions>();

            await builder.Build().RunAsync();
        }
    }
}
