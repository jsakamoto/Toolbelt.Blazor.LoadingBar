using System.Linq;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Toolbelt.Blazor
{
    public static class HttpClientInterceptorExtension
    {
        public static void AddHttpClientInterceptor(this IServiceCollection services)
        {
            if (services.FirstOrDefault(d => d.ServiceType == typeof(HttpClientInterceptor)) == null)
            {
                services.AddSingleton(_ => new HttpClientInterceptor());
            }
        }

        public static IBlazorApplicationBuilder UseHttpClientInterceptor(this IBlazorApplicationBuilder app)
        {
            var interceptor = app.Services.GetService<HttpClientInterceptor>();
            interceptor.Install(app);

            return app;
        }
    }
}
