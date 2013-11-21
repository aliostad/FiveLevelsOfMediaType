using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Xunit;

namespace FiveLevelsOfMediaType.Tests.Integration
{
    public class MediaTypeBasedActionSelectorTests
    {
        [Fact]
        public void SelectsSuperItemBasedOnParameter_NonCanonical()
        {
            // arrange
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IHttpActionSelector), new MediaTypeBasedActionSelector());
            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            configuration.AddFiveLevelsOfMediaType();
            var server = new InMemoryServer(configuration);
            var client = new HttpClient(server);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://lhasa/api/Item/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/SuperItem+json"));

            // act
            var response = client.SendAsync(request).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var mediaType = response.Content.Headers.ContentType.ExtractFiveLevelsOfMediaType();

            // assert
            Assert.Equal("SuperItem", mediaType.DomainModel);
        }

        [Fact]
        public void SelectsSuperItemBasedOnParameter_5LMT()
        {
            // arrange
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IHttpActionSelector), new MediaTypeBasedActionSelector());
            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            configuration.AddFiveLevelsOfMediaType();
            var server = new InMemoryServer(configuration);
            var client = new HttpClient(server);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://lhasa/api/Item/1");
            var headerValue = new MediaTypeWithQualityHeaderValue("application/json");
            headerValue.Parameters.Add(new NameValueHeaderValue("domain-model", "SuperItem"));
            request.Headers.Accept.Add(headerValue);

            // act
            var response = client.SendAsync(request).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var mediaType = response.Content.Headers.ContentType.ExtractFiveLevelsOfMediaType();

            // assert
            Assert.Equal("SuperItem", mediaType.DomainModel);
        }

    }
}
