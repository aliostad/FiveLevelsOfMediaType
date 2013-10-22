using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace FiveLevelsOfMediaType
{  

    public class FiveLevelsOfMediaTypeFormatter : MediaTypeFormatter
    {
        private MediaTypeFormatter _internalFormatter;

        public FiveLevelsOfMediaTypeFormatter(MediaTypeFormatter internalFormatter)
        {
            _internalFormatter = internalFormatter;
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
            Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext)
                .ContinueWith(t =>
                                  {
                                      if (!t.IsFaulted)
                                      {
                                          content.Headers.ContentType
                                              .AddFiveLevelsOfMediaType(type);
                                      }
                                      return t;

                                  });
        }
    }


       
}