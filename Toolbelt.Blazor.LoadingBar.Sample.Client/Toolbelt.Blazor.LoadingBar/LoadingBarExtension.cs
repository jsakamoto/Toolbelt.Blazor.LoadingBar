using System;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor
{
    public static class LoadingBarExtension
    {
        private static bool Installed;

        public static IBlazorApplicationBuilder UseLoadingBar(this IBlazorApplicationBuilder app)
        {
            if (Installed) return app;

            app.UseHttpClientInterceptor();

            var interceptor = app.Services.GetService<HttpClientInterceptor>();
            interceptor.BeforeSend += Interceptor_BeforeSend;
            interceptor.AfterSend += Interceptor_AfterSend;

            Installed = true;
            return app;
        }

        private static void Interceptor_BeforeSend(object sender, EventArgs e)
        {
            JSRuntime.Current?.InvokeAsync<object>("Toolbelt.Blazor.loadingBar.beforeSend");
        }

        private static void Interceptor_AfterSend(object sender, EventArgs e)
        {
            JSRuntime.Current?.InvokeAsync<object>("Toolbelt.Blazor.loadingBar.afterSend");
        }
    }
}
