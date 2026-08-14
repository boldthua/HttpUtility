using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtility
{
    internal class HttpHandler : DelegatingHandler // AOP 切片
    {
        IHttpInterceptor interceptor;
        public HttpHandler(IHttpInterceptor interceptor, bool isUseProxy = false)
        {
            this.interceptor = interceptor;
            var handler = new HttpClientHandler();
            this.InnerHandler = handler;
            if (isUseProxy)
            {
                handler.UseProxy = isUseProxy;
                handler.Proxy = new WebProxy("http://127.0.0.1:8888");
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            InterceptionResult intercept = await interceptor.HandleAsync(request, base.SendAsync);

            if (intercept.RequestToSend != null)
            {
                return await base.SendAsync(request, cancellationToken);
            }
            return intercept.ShortCircuitResponse;
        }
    }
}
