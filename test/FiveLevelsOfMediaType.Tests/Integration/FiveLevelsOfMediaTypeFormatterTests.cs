using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;
using Xunit.Extensions;

namespace FiveLevelsOfMediaType.Tests.Integration
{
    public class FiveLevelsOfMediaTypeFormatterTests
    {
        [Fact]
        public void FiveLevelsAddsParams()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.AddFiveLevelsOfMediaType();
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new ObjectContent<Person>(new Person(), httpConfiguration.Formatters[0]);
            var handler = new DummyMessageHandler() {Response = response};
            var httpServer = new HttpServer(httpConfiguration, handler);
            var invoker = new HttpMessageInvoker(httpServer);
            var result = invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/ddd"), 
                CancellationToken.None).Result;


            Assert.True(result.Content.Headers.ContentType.Parameters.Any(x => x.Name == "is-text"));
        }
    }


    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
