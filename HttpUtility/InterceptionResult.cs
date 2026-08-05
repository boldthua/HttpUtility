using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtility
{
    public class InterceptionResult
    {
        /// <summary>
        /// 若為 null：沿用原始 request。
        /// 若不為 null：BaseHandler 將使用這個 request 替代。
        /// </summary>
        public HttpRequestMessage RequestToSend { get; private set; }
        /// <summary>
        /// 若不為 null：短路整個流程，直接回這個 response。
        /// </summary>
        public HttpResponseMessage ShortCircuitResponse { get; private set; }

        /// <summary>
        /// 替換掉原始 request，改用新的 request 發送。
        /// </summary>
        public static InterceptionResult ReplaceRequest(HttpRequestMessage newRequest) => new InterceptionResult { RequestToSend = newRequest };
        /// <summary>
        /// 短路，不發送 request，直接回傳指定 response。
        /// </summary>
        public static InterceptionResult CancelRequest(HttpResponseMessage response) => new InterceptionResult { ShortCircuitResponse = response };

    }

}
