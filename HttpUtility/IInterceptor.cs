using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtility
{
    internal interface IInterceptor
    {
        Task<Interceptor> HandleAsync(
            HttpRequestMessage request, 
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync
            );
    }
}
