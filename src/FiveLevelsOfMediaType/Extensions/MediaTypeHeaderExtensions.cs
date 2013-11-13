using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;


namespace FiveLevelsOfMediaType
{
    public static class MediaTypeHeaderExtensions
    {

      

        public static void AddFiveLevelsOfMediaType(this MediaTypeHeaderValue header,Type type,
             params string[] nonCanonicalMediaTypePatterns)            
        {
            var extendedMediaType = GetExtendedMediaType(header.MediaType, nonCanonicalMediaTypePatterns);

            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.DomainModel,
                extendedMediaType == null || string.IsNullOrEmpty(extendedMediaType.DomainModel) ?
                HttpUtility.UrlEncode(type.Name) :
                extendedMediaType.DomainModel
            ));

            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Version,
                extendedMediaType == null || string.IsNullOrEmpty(extendedMediaType.Version) ?
                HttpUtility.UrlEncode(type.Assembly.GetName().Version.ToString()) :
                extendedMediaType.Version));

            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Format,
                extendedMediaType == null || string.IsNullOrEmpty(extendedMediaType.Format) ?
                HttpUtility.UrlEncode(header.MediaType) :
                HttpUtility.UrlEncode(extendedMediaType.Format)));

            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Schema,
                extendedMediaType == null || string.IsNullOrEmpty(extendedMediaType.Schema) ?
                HttpUtility.UrlEncode(header.MediaType) :
                HttpUtility.UrlEncode(extendedMediaType.Schema)));

            header.Parameters.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.IsText,
                extendedMediaType == null || !extendedMediaType.IsText.HasValue ? 
                "" :
                extendedMediaType.IsText.Value.ToString()));

        }

       

        public static ExtendedMediaType ExtractFiveLevelsOfMediaType(this MediaTypeHeaderValue header,
            params string[] nonCanonicalMediaTypePatterns)
        {
            if (header == null)
                return null;

            var extendedMediaType = GetExtendedMediaType(header.MediaType, nonCanonicalMediaTypePatterns);

            if (extendedMediaType != null)
                return extendedMediaType;


            if (header.Parameters.Count == 0)
                return null;

            extendedMediaType = new ExtendedMediaType();
            var domainModel = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.DomainModel));
            var format = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.Format));
            var isText = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.IsText));
            var schema = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.Schema));
            var version = header.Parameters.FirstOrDefault(x => x.Name.Equals(FiveLevelsOfMediaTypeParameters.Version));
            extendedMediaType.DomainModel = domainModel == null ? string.Empty : HttpUtility.UrlDecode(
                domainModel.Value);
            extendedMediaType.Format = format == null ? string.Empty : HttpUtility.UrlDecode(format.Value);
            extendedMediaType.IsText = isText == null ? null : (bool?) Convert.ToBoolean(isText.Value);
            extendedMediaType.Schema = schema == null ? string.Empty : HttpUtility.UrlDecode(schema.Value);
            extendedMediaType.Version = version == null ? string.Empty : HttpUtility.UrlDecode(version.Value);

            return extendedMediaType;
        }


        internal static ExtendedMediaType GetExtendedMediaType(string mediaType,
            params string[] nonCanonicalMediaTypePatterns)
        {

            Match match = null;
            var matched = nonCanonicalMediaTypePatterns.Any(mtp =>
            {
                match = Regex.Match(mediaType, mtp);
                return match.Success;
            });



            if (matched)
            {
                var extendedMediaType = new ExtendedMediaType();
                extendedMediaType.DomainModel = match.Groups[3].Value;
                extendedMediaType.Version = match.Groups[5].Value;
                extendedMediaType.Format = match.Groups[1].Value + "/" + match.Groups[6].Value;
                extendedMediaType.Schema = string.IsNullOrEmpty(match.Groups[3].Value) ? extendedMediaType.Format : 
                    string.Format("{0}/{1}+{2}",
                    match.Groups[1].Value,
                    match.Groups[3].Value,
                    match.Groups[6].Value);
                extendedMediaType.IsText = 
                    match.Groups[1].Value.ToLower().IsIn(new []{"text"}) ||
                    match.Groups[6].Value.ToLower().IsIn(new[] { "xml", "json", "xml-dtd", 
                        "javascript", "http", "rfc822" });

                return extendedMediaType;
            }
            else
            {
                return null;
            }
        }

 
    }
}