using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor
{
    /// <summary>
    /// Implementation of loading bar UI.
    /// </summary>
    public class LoadingBar : IDisposable
    {
        internal readonly LoadingBarOptions Options = new LoadingBarOptions();

        private readonly HttpClientInterceptor Interceptor;

        private readonly IJSRuntime JSRuntime;

        private Task LastTask;

        /// <summary>
        /// Initialize LoadingBar service instance.
        /// </summary>
        public LoadingBar(HttpClientInterceptor interceptor, IJSRuntime jSRuntime)
        {
            this.Interceptor = interceptor;
            this.JSRuntime = jSRuntime;
            interceptor.BeforeSend += Interceptor_BeforeSend;
            interceptor.AfterSend += Interceptor_AfterSend;
        }

        private void Interceptor_BeforeSend(object sender, EventArgs e) => BeginLoading();

        private void Interceptor_AfterSend(object sender, EventArgs e) => EndLoading();

        internal void ConstructDOM()
        {
            if (this.JSRuntime == null) return;
            this.LastTask = this.ConstructDOMAsync();
        }

        private async Task ConstructDOMAsync()
        {
            if (!Options.DisableStyleSheetAutoInjection)
            {
                const string cssPath = "_content/Toolbelt.Blazor.LoadingBar/style.min.css";
                await this.JSRuntime.InvokeVoidAsync("eval", "((d,s)=>(h=>h.querySelector(`link[href=\"${s}\"]`)?0:(e=>(e.href=s,e.rel='stylesheet',h.insertAdjacentElement('afterBegin',e)))(d.createElement('link')))(d.head))(document,'" + cssPath + "')");
            }

            if (!Options.DisableClientScriptAutoInjection)
            {
                const string scriptPath = "_content/Toolbelt.Blazor.LoadingBar/script.min.js";
                await this.JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s)=>(h=>h.querySelector(t+`[src=\"${s}\"]`)?r():(e=>(e.src=s,e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "'))");
            }

            await JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.loadingBar.constructDOM", this.Options.LoadingBarColor);
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
            if (this.JSRuntime == null) return;
            var task = this.LastTask ?? Task.CompletedTask;
            this.LastTask = task.ContinueWith(_ => JSRuntime.InvokeVoidAsync("eval", "Toolbelt.Blazor.loadingBar." + methodName + "()"));
        }

        /// <summary>
        /// Unsubscribe events that are provided from HttpClientInterceptor.
        /// </summary>
        public void Dispose()
        {
            Interceptor.BeforeSend -= Interceptor_BeforeSend;
            Interceptor.AfterSend -= Interceptor_AfterSend;
        }
    }
}
