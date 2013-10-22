using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;


namespace FiveLevelsOfMediaType
{
    internal static class MediaTypeHeaderExtensions
    {
        public static void AddFiveLevelsOfMediaType(this MediaTypeHeaderValue header,Type type)
        {
            header.Parameters.Clear();
            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.DomainModel,
                type.FullName));
            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Version,
                type.Assembly.GetName().Version.ToString()));
            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Format,
                ExtractFormat(header.MediaType).ReplaceHttpSeparators()));
            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Schema,
                header.MediaType.ReplaceHttpSeparators()));
            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.IsText,
                "true")); // TODO: a map of textual content types


        }

        internal static string ExtractFormat(string mediaType)
        {
            const string NonCanonicalSchemaFormat = @"^([\w\d.]+)/(?:([\w\d.]+)\+)?([\w\d.]+)$";

            var match = Regex.Match(mediaType, NonCanonicalSchemaFormat);
            return match.Success
                       ? string.Format("{0}/{1}", match.Groups[1].Value, match.Groups[3].Value)
                       : mediaType;
        }
 
    }
}