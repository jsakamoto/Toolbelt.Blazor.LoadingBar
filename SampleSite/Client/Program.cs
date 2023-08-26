﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleSite.Client.Service;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace SampleSite.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddLoadingBarService(options =>
            {
                options.LoadingBarColor = "yellow";
                options.LatencyThreshold = 100;
                //options.ContainerSelector = "#custom-loadingbar-container";
                //options.DisableStyleSheetAutoInjection = true;
                //options.DisableClientScriptAutoInjection = true;
            });

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }.EnableIntercept(sp));

            // Typed HttpClient
            // See also: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1#typed-clients
            builder.Services.AddHttpClient<WeatherForecastService>((sp, client) =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                // If you want to use Typed Client, please invoke "EnableIntercept()" here.
                client.EnableIntercept(sp);
            })
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Demonstration for adding an authorization message handler.
            builder.Services.TryAddTransient<BaseAddressAuthorizationMessageHandler>();
            builder.Services.TryAddTransient<IAccessTokenProvider, DummyAccessTokenProvider>();

            builder.UseLoadingBar();
            await builder.Build().RunAsync();
        }
    }
}
