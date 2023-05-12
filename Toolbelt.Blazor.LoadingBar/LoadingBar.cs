using System;
using System.ComponentModel;
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

        private Task? ConstructDOMTask;

        private Task? LastTask;

        private Func<string, object[], ValueTask>? JSInvoker;

        /// <summary>
        /// Initialize LoadingBar service instance.
        /// </summary>
        public LoadingBar(HttpClientInterceptor interceptor, IJSRuntime jSRuntime)
        {
            this.Interceptor = interceptor;
            this.JSRuntime = jSRuntime;
            interceptor.BeforeSendAsync += this.Interceptor_BeforeSendAsync;
            interceptor.AfterSend += this.Interceptor_AfterSend;
        }

        private Task Interceptor_BeforeSendAsync(object? sender, EventArgs e) => this.BeginLoadingAsync();

        private void Interceptor_AfterSend(object? sender, EventArgs e) => this.EndLoading();

        internal void ConstructDOM()
        {
            this.ConstructDOMTask = this.ConstructDOMAsync();
            this.LastTask = this.ConstructDOMTask;
        }

        private async Task ConstructDOMAsync()
        {
            var version = this.GetVersionText();
#if ENABLE_JSMODULE
            if (!this.Options.DisableClientScriptAutoInjection)
            {
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
                var scriptPath = "_content/Toolbelt.Blazor.LoadingBar/script.min.js";
                await this.JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s,v)=>(h=>h.querySelector(t+`[src^=\"${s}\"]`)?r():(e=>(e.src=(s+v),e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "','?v=" + version + "'))");
            }
            await this.JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.loadingBarReady");
            this.JSInvoker = this.JSRuntime.InvokeVoidAsync;
#endif
            var cssPath = "_content/Toolbelt.Blazor.LoadingBar/style.min.css";
            await this.JSInvoker("Toolbelt.Blazor.loadingBar.constructDOM", new object[] {
                this.Options.LoadingBarColor,
                this.Options.DisableStyleSheetAutoInjection ? "" : cssPath,
                version,
                this.Options.ContainerSelector,
                this.Options.LatencyThreshold
            });
        }

        /// <summary>
        /// Begin loading bar UI.
        /// </summary>
        [Obsolete("Use BeginLoadingAsync instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public void BeginLoading()
        {
            var _ = this.BeginLoadingAsync();
        }

        /// <summary>
        /// Begin loading bar UI.
        /// </summary>
        public async Task BeginLoadingAsync()
        {
            if (this.ConstructDOMTask != null && !this.ConstructDOMTask.IsCompleted)
            {
                await this.ConstructDOMTask;
            }
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
            if (this.JSInvoker == null) return;
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
            this.Interceptor.BeforeSendAsync -= this.Interceptor_BeforeSendAsync;
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
