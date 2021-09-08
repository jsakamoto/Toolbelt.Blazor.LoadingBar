using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor
{
    /// <summary>
    /// Implementation of loading bar UI.
    /// </summary>
    public class LoadingBar : IDisposable
#if ENABLE_JSMODULE
        , IAsyncDisposable
#endif
    {
        internal readonly LoadingBarOptions Options = new LoadingBarOptions();

        private readonly HttpClientInterceptor Interceptor;

        private readonly IJSRuntime JSRuntime;

        private Task? LastTask;

        private Func<string, object[], ValueTask>? JSInvoker;

        /// <summary>
        /// Initialize LoadingBar service instance.
        /// </summary>
        public LoadingBar(HttpClientInterceptor interceptor, IJSRuntime jSRuntime)
        {
            this.Interceptor = interceptor;
            this.JSRuntime = jSRuntime;
            interceptor.BeforeSend += this.Interceptor_BeforeSend;
            interceptor.AfterSend += this.Interceptor_AfterSend;
        }

        private void Interceptor_BeforeSend(object? sender, EventArgs e) => this.BeginLoading();

        private void Interceptor_AfterSend(object? sender, EventArgs e) => this.EndLoading();

        internal void ConstructDOM()
        {
            this.LastTask = this.ConstructDOMAsync();
        }

        private async Task ConstructDOMAsync()
        {
            if (!this.Options.DisableStyleSheetAutoInjection)
            {
                const string cssPath = "_content/Toolbelt.Blazor.LoadingBar/style.min.css";
                await this.JSRuntime.InvokeVoidAsync("eval", "((d,s)=>(h=>h.querySelector(`link[href=\"${s}\"]`)?0:(e=>(e.href=s,e.rel='stylesheet',h.insertAdjacentElement('afterBegin',e)))(d.createElement('link')))(d.head))(document,'" + cssPath + "')");
            }

#if ENABLE_JSMODULE
            if (!this.Options.DisableClientScriptAutoInjection)
            {
                var version = this.GetVersionText();
                var scriptPath = $"./_content/Toolbelt.Blazor.LoadingBar/script.module.min.js?v={version}";
                this.JSModule = await this.JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
                this.JSInvoker = this.JSModule.InvokeVoidAsync;
            }
            else
            {
                await this.JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.loadingBarReady");
                this.JSInvoker = this.JSRuntime.InvokeVoidAsync;
            }
#else
            if (!this.Options.DisableClientScriptAutoInjection)
            {
                var version = this.GetVersionText();
                var scriptPath = $"_content/Toolbelt.Blazor.LoadingBar/script.min.js?v={version}";
                await this.JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s)=>(h=>h.querySelector(t+`[src^=\"${s}\"]`)?r():(e=>(e.src=s,e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "'))");
            }
            await this.JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.loadingBarReady");
            this.JSInvoker = this.JSRuntime.InvokeVoidAsync;
#endif
            await this.JSInvoker("Toolbelt.Blazor.loadingBar.constructDOM", new object[] { this.Options.LoadingBarColor });
        }

        /// <summary>
        /// Begin loading bar UI.
        /// </summary>
        public void BeginLoading()
        {
            this.InvokeJS("beginLoading");
        }

        /// <summary>
        /// End loading bar UI.
        /// </summary>
        public void EndLoading()
        {
            this.InvokeJS("endLoading");
        }

        private void InvokeJS(string methodName)
        {
            if (JSInvoker == null) return;
            var task = this.LastTask ?? Task.CompletedTask;
            this.LastTask = task.ContinueWith(_ => this.JSInvoker("Toolbelt.Blazor.loadingBar." + methodName, Array.Empty<object>()));
        }

        private string GetVersionText()
        {
            var assembly = this.GetType().Assembly;
            var version = assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "";
            return version;
        }

        /// <summary>
        /// Unsubscribe events that are provided from HttpClientInterceptor.
        /// </summary>
        public void Dispose()
        {
            this.Interceptor.BeforeSend -= this.Interceptor_BeforeSend;
            this.Interceptor.AfterSend -= this.Interceptor_AfterSend;
        }

#if ENABLE_JSMODULE

        private IJSObjectReference? JSModule;

        public async ValueTask DisposeAsync()
        {
            if (this.JSModule != null) await this.JSModule.DisposeAsync();
        }
#endif
    }
}
