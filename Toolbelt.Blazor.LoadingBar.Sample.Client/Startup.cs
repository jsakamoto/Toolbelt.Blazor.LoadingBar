using System;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Toolbelt.Blazor.LoadingBar.Sample.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClientInterceptor();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.UseLoadingBar();

            app.AddComponent<App>("app");
        }
    }
}
