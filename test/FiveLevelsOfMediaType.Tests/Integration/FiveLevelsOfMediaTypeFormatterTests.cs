using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
            // set up
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.AddFiveLevelsOfMediaType();
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new ObjectContent<Person>(new Person(), httpConfiguration.Formatters[0]);

            // run 
            var result = GetResponse(httpConfiguration, response);

            var dictionary = result.Content.Headers.ContentType.Parameters.ToDictionary(x => x.Name, y => y.Value);

            // assert
            Assert.Equal(true.ToString(), dictionary[FiveLevelsOfMediaTypeParameters.IsText]);
            Assert.Equal("Person", dictionary[FiveLevelsOfMediaTypeParameters.DomainModel]);
            Assert.Equal(Assembly.GetExecutingAssembly()
                                 .GetName().Version.ToString(), dictionary[FiveLevelsOfMediaTypeParameters.Version]);
            Assert.Equal("application/json", HttpUtility.UrlDecode(dictionary[FiveLevelsOfMediaTypeParameters.Format]));
            Assert.Equal("application/json", HttpUtility.UrlDecode(dictionary[FiveLevelsOfMediaTypeParameters.Schema]));
        }

     


        private static HttpResponseMessage GetResponse(HttpConfiguration configuration, HttpResponseMessage initialResponse)
        {
            configuration.AddFiveLevelsOfMediaType();
            var handler = new DummyMessageHandler() { Response = initialResponse };
            var httpServer = new HttpServer(configuration, handler);
            var invoker = new HttpMessageInvoker(httpServer);
            return invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/ddd"),
                                           CancellationToken.None).Result;

        }

    }

    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
