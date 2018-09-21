using System.Linq;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Toolbelt.Blazor.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding and using LoadingBar.
    /// </summary>
    public static class LoadingBarExtension
    {
        /// <summary>
        ///  Adds a LoadingBar service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        public static void AddLoadingBar(this IServiceCollection services)
        {
            services.AddHttpClientInterceptor();

            if (services.FirstOrDefault(d => d.ServiceType == typeof(LoadingBar)) == null)
            {
                services.AddSingleton<LoadingBar>();
            }
        }

        private static bool Installed;

        /// <summary>
        ///  Installs a LoadingBar service to the runtime hosting environment.
        /// </summary>
        /// <param name="app">The Microsoft.AspNetCore.Blazor.Builder.IBlazorApplicationBuilder.</param>
        public static IBlazorApplicationBuilder UseLoadingBar(this IBlazorApplicationBuilder app)
        {
            if (Installed) return app;

            app.UseHttpClientInterceptor();

            var loadinBar = app.Services.GetService<LoadingBar>();
            loadinBar.ConstructDOM();

            Installed = true;
            return app;
        }
    }
}
