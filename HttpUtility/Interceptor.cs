using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtility
{
    internal class Interceptor : IInterceptor
    {
        /// <summary>
        /// 若為 null：沿用原始 request。
        /// 若不為 null：BaseHandler 將使用這個 request 替代。
        /// </summary>
        public HttpRequestMessage RequestToSend {  get; private set; }
        /// <summary>
        /// 若不為 null：短路整個流程，直接回這個 response。
        /// </summary>
        public HttpResponseMessage ShortCircuitResponse { get; private set; }

        public Task<Interceptor> HandleAsync(HttpRequestMessage request = null, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
        {
            if (request == null)
            {
                // 執行Func，生成response並回傳
            }
            Interceptor newInterceptor = new Interceptor(); 
            newInterceptor.RequestToSend = request;
            return Task.FromResult(newInterceptor);

        }
        /// <summary>
        /// 替換掉原始 request，改用新的 request 發送。
        /// </summary>
        public static Interceptor ReplaceRequest(HttpRequestMessage newRequest) => new Interceptor { RequestToSend = newRequest };
        /// <summary>
        /// 短路，不發送 request，直接回傳指定 response。
        /// </summary>
        public static Interceptor CancelRequest(HttpResponseMessage response) => new Interceptor { ShortCircuitResponse = response };

        }
    
}
