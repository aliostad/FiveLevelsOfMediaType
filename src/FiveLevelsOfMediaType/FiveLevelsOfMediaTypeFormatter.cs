using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly List<string> _supportedNonCanonicalMediaTypePatterns = new List<string>();

        public static string DefaultNonCanonicalMediaTypePattern =
            @"^([\w\.]+)/(([a-zA-Z\.]+)(?:([\-\.])v?([\d\.]+))?\+)?([\w\.-]+)$";

        public FiveLevelsOfMediaTypeFormatter(MediaTypeFormatter internalFormatter)
        {
            _internalFormatter = internalFormatter;
            _internalFormatter.SupportedEncodings.Each(x=> SupportedEncodings.Add(x));
            _internalFormatter.SupportedMediaTypes.Each(x => SupportedMediaTypes.Add(x));
            _internalFormatter.MediaTypeMappings.Each(x => MediaTypeMappings.Add(x));

            _supportedNonCanonicalMediaTypePatterns.Add(
              DefaultNonCanonicalMediaTypePattern);
        }

        public override bool CanReadType(Type type)
        {
            return _internalFormatter.CanReadType(type);
        }

        public override bool CanWriteType(Type type)
        {
            return _internalFormatter.CanWriteType(type);
        }

        public override Task WriteToStreamAsync(Type type, object value, 
            Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return _internalFormatter.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, System.Threading.CancellationToken cancellationToken)
        {
            return _internalFormatter.WriteToStreamAsync(type, value, writeStream, content, transportContext, cancellationToken);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return _internalFormatter.ReadFromStreamAsync(type, readStream, content, formatterLogger);
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, 
            MediaTypeHeaderValue mediaType)
        {
            var instance = _internalFormatter.GetPerRequestFormatterInstance(type, request, mediaType);
            if(instance==null)
                return this;
            else
                return new FiveLevelsOfMediaTypeFormatter(instance);
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

        public ICollection<string> SupportedNonCanonicalMediaTypePatterns
        {
            get { return _supportedNonCanonicalMediaTypePatterns; }
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            _internalFormatter.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType.AddFiveLevelsOfMediaType(type, DefaultNonCanonicalMediaTypePattern);
        }

    }


       
}