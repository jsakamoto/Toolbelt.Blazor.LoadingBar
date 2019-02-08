using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace SampleSite.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLoadingBar();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.UseLoadingBar();

            app.AddComponent<App>("app");
        }
    }
}
