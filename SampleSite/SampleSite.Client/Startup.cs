using System;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor;

namespace SampleSite.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLoadingBar();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.UseLoadingBar();

            app.AddComponent<App>("app");
        }
    }
}
