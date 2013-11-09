using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FiveLevelsOfMediaType.Tests.Integration
{
    class DummyMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var completionSource = new TaskCompletionSource<HttpResponseMessage>();
            completionSource.SetResult(Response);
            return completionSource.Task;
        }

        public HttpResponseMessage Response { get; set; }
    }
}
