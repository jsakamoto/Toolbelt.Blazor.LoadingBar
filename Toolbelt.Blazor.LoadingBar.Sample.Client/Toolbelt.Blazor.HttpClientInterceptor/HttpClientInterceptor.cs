using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Toolbelt.Blazor
{
    public class HttpClientInterceptor : HttpMessageHandler
    {
        public event EventHandler BeforeSend;

        public event EventHandler AfterSend;

        private HttpMessageHandler Handler;

        private readonly MethodInfo SendAsyncMethod;

        internal HttpClientInterceptor()
        {
            this.SendAsyncMethod = typeof(HttpMessageHandler).GetMethod(nameof(SendAsync), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        internal void Install(IBlazorApplicationBuilder app)
        {
            if (this.Handler != null) return;

            var httpClient = app.Services.GetService<HttpClient>();
            var handlerField = typeof(HttpMessageInvoker).GetField("handler", BindingFlags.Instance | BindingFlags.NonPublic);
            this.Handler = handlerField.GetValue(httpClient) as HttpMessageHandler;

            handlerField.SetValue(httpClient, this);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                this.BeforeSend?.Invoke(this, EventArgs.Empty);
                return await (this.SendAsyncMethod.Invoke(this.Handler, new object[] { request, cancellationToken }) as Task<HttpResponseMessage>);
            }
            finally
            {
                this.AfterSend?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
