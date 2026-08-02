using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtility
{
    internal class HttpHandler:DelegatingHandler
    {
        IInterceptor interceptor;
        public HttpHandler(IInterceptor interceptor)
        {
            this.interceptor = interceptor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Interceptor intercept = await interceptor.HandleAsync(request,base.SendAsync);

            return intercept.ShortCircuitResponse;
        }
    }
}
