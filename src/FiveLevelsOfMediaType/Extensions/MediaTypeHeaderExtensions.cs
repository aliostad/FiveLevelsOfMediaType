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

        public static ExtendedMediaType ExtractFiveLevelsOfMediaType(this MediaTypeHeaderValue header)
        {
            if (header == null)
                return null;

            if (header.Parameters.Count == 0)
                return null;

            var extendedMediaType = new ExtendedMediaType();
            var domainModel = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.DomainModel));
            var format = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.Format));
            var isText = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.IsText));
            var schema = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.Schema));
            var version = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.Version));
            extendedMediaType.DomainModel = domainModel == null ? string.Empty : domainModel.Value;
            extendedMediaType.Format = format == null ? string.Empty : format.Value;
            extendedMediaType.IsText = isText == null ? null : (bool?) Convert.ToBoolean(isText.Value);
            extendedMediaType.Schema = schema == null ? string.Empty : schema.Value;
            extendedMediaType.Version = version == null ? string.Empty : version.Value;

            return extendedMediaType;
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