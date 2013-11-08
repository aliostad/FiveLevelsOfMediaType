using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FiveLevelsOfMediaType
{  

    public class FiveLevelsOfMediaTypeFormatter : MediaTypeFormatter
    {
        private MediaTypeFormatter _internalFormatter;

        public FiveLevelsOfMediaTypeFormatter(MediaTypeFormatter internalFormatter)
        {
            _internalFormatter = internalFormatter;
            _internalFormatter.SupportedEncodings.Each(x=> SupportedEncodings.Add(x));
            _internalFormatter.SupportedMediaTypes.Each(x => SupportedMediaTypes.Add(x));
            _internalFormatter.MediaTypeMappings.Each(x => MediaTypeMappings.Add(x));
        }

        public override bool CanReadType(Type type)
        {
            return _internalFormatter.CanReadType(type);
        }

        public override bool CanWriteType(Type type)
        {
            return _internalFormatter.CanWriteType(type);
        }

        public override async Task WriteToStreamAsync(Type type, object value, 
            Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            await _internalFormatter.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            content.Headers.ContentType.AddFiveLevelsOfMediaType(type);
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, System.Threading.CancellationToken cancellationToken)
        {
            await _internalFormatter.WriteToStreamAsync(type, value, writeStream, content, transportContext, cancellationToken);
            content.Headers.ContentType.AddFiveLevelsOfMediaType(type);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return _internalFormatter.ReadFromStreamAsync(type, readStream, content, formatterLogger);
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, System.Net.Http.Headers.MediaTypeHeaderValue mediaType)
        {
            return _internalFormatter.GetPerRequestFormatterInstance(type, request, mediaType);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger, System.Threading.CancellationToken cancellationToken)
        {
            return _internalFormatter.ReadFromStreamAsync(type, readStream, content, formatterLogger, cancellationToken);
        }

        public override IRequiredMemberSelector RequiredMemberSelector
        {
            get
            {
                return _internalFormatter.RequiredMemberSelector;
            }
            set
            {
                _internalFormatter.RequiredMemberSelector = value;
            }
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            _internalFormatter.SetDefaultContentHeaders(type, headers, mediaType);
        }

    }


       
}