using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FiveLevelsOfMediaType.Tests.Integration;
using Xunit;
using Xunit.Extensions;

namespace FiveLevelsOfMediaType.Tests
{
    public class MediaTypeParsing
    {
        [Theory]
        [InlineData("application/vnd.mozilla.xul+xml", "True", "application/xml", "application/vnd.mozilla.xul+xml", "vnd.mozilla.xul", "")]
        [InlineData("application/vnd.mozilla.xul.1.0.0.0+xml", "True", "application/xml", "application/vnd.mozilla.xul+xml", "vnd.mozilla.xul", "1.0.0.0")]
        [InlineData("application/xml-dtd", "True", "application/xml-dtd", "application/xml-dtd", "", "")]
        [InlineData("application/vnd.mozilla.xul.v1.0.0.0+xml", "True", "application/xml", "application/vnd.mozilla.xul+xml", "vnd.mozilla.xul", "1.0.0.0")]
        [InlineData("application/vnd.mozilla.xul.v12+xml", "True", "application/xml", "application/vnd.mozilla.xul+xml", "vnd.mozilla.xul", "12")]
        public void ParseTest(
            string mediaType,
            string isText,
            string format,
            string schema,
            string domainModel,
            string version)
        {
            // set up


            var extendedMediaType = MediaTypeHeaderExtensions.GetExtendedMediaType(mediaType, 
               new [] { FiveLevelsOfMediaTypeFormatter.DefaultNonCanonicalMediaTypePattern});

            // assert
            Assert.Equal(isText, extendedMediaType.IsText.ToString());
            Assert.Equal(domainModel, extendedMediaType.DomainModel);
            Assert.Equal(version, extendedMediaType.Version);
            Assert.Equal(format, extendedMediaType.Format);
            Assert.Equal(schema, extendedMediaType.Schema);
        }
    }
}
