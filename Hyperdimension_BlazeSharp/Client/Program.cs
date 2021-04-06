using Hyperdimension_BlazeSharp.Client.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddSingleton<CompileService>();
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            // TODO: Use HttpClientFactory to eliminate Socket Exhaustion
            builder.Services.AddSingleton<TasksHistoryDraft>();
            builder.Services.AddSingleton<IProfileViewModel, ProfileViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
