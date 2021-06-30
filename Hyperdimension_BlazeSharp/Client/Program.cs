using Hyperdimension_BlazeSharp.Client.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
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

            builder.Services.AddOptions();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            builder.Services.AddSingleton<CompileService>();            
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddScoped<TasksHistoryDraft>();
            builder.Services.AddHttpClient<IProfileViewModel, ProfileViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<ILearningPathViewModel, LearningPathViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<IRankingViewModel, RankingViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<ITaskPlaygroundViewModel, TaskPlaygroundViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<ILoginViewModel, LoginViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<IRegisterViewModel, RegisterViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<ICustomTaskViewModel, CustomTaskViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<ICustomModuleViewModel, CustomModuleViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<IForumViewModel, ForumViewModel>
                ("HyperdimensionBlazeSharp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            

            await builder.Build().RunAsync();
        }
    }
}
