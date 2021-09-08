using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.JSInterop;

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
            services.AddLoadingBar(configure: null);
        }

        /// <summary>
        ///  Adds a LoadingBar service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        /// <param name="configure"></param>
        public static void AddLoadingBar(this IServiceCollection services, Action<LoadingBarOptions>? configure)
        {
            services.AddHttpClientInterceptor();
            services.TryAddSingleton(sp =>
            {
                var loadingBar = new LoadingBar(
                    sp.GetRequiredService<HttpClientInterceptor>(),
                    sp.GetRequiredService<IJSRuntime>());
                configure?.Invoke(loadingBar.Options);
                return loadingBar;
            });
        }

        private static bool Installed;

        /// <summary>
        ///  Installs a LoadingBar service to the runtime hosting environment.
        /// </summary>
        /// <param name="host">The Microsoft.AspNetCore.Blazor.Hosting.WebAssemblyHost.</param>
        [Obsolete("Use \"builder.UserLoadingBar();\" instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static WebAssemblyHost UseLoadingBar(this WebAssemblyHost host)
        {
            if (Installed) return host;

            var loadinBar = host.Services.GetRequiredService<LoadingBar>();
            loadinBar.ConstructDOM();

            Installed = true;
            return host;
        }

        /// <summary>
        ///  Installs a LoadingBar service to the runtime hosting environment.
        /// </summary>
        /// <param name="hostBuilder">The Microsoft.AspNetCore.Blazor.Hosting.WebAssemblyHostBuilder.</param>
        public static WebAssemblyHostBuilder UseLoadingBar(this WebAssemblyHostBuilder hostBuilder)
        {
            if (!Installed)
            {
                hostBuilder.RootComponents.Add<ScriptInjectorComponent>("script[src^='_framework/blazor']");
            }
            Installed = true;
            return hostBuilder;
        }
    }
}
