using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using ApiActionSelection.System.Web.Http.Controllers;

namespace FiveLevelsOfMediaType
{
    public class MediaTypeBasedActionSelector : ApiActionSelector
    {
        private readonly Func<string, string> _domainNameToTypeNameMapper;

        public MediaTypeBasedActionSelector()
            : this(DefaultMapper)
        {
            
        }


        /// <summary>
        /// Creates an instance of MediaTypeBasedActionSelector
        /// </summary>
        /// <param name="domainNameToTypeNameMapper">A function that extracts type name from
        /// non-canonical doamin-name. For example if domain name is vnd.MyCompany.MyType then it
        /// returns MyType. Or it just maps it to any arbitrary value.
        /// Default mapper takes the token after the last period.</param>
        public MediaTypeBasedActionSelector(Func<string, string> domainNameToTypeNameMapper)
        {
            _domainNameToTypeNameMapper = domainNameToTypeNameMapper;
        }

        private static string DefaultMapper(string domainName)
        {
            var lastIndexOf = domainName.LastIndexOf('.');
            if (lastIndexOf < 0)
                return domainName;
            else
            {
                return domainName.Substring(lastIndexOf + 1);
            }
        }

        protected override ReflectedHttpActionDescriptor ResolveAmbiguousActions(HttpControllerContext context,
            IEnumerable<ReflectedHttpActionDescriptor> actionDescriptors)
        {

            // normally for POST and PUT that we resolve with content type
            if (context.Request.Content != null && context.Request.Content.Headers.ContentType != null)
            {
                var extendedMedaType = context.Request.Content.Headers.ContentType.ExtractFiveLevelsOfMediaType();
                if (extendedMedaType != null)
                {
                    var candidate = actionDescriptors.FirstOrDefault(x =>
                                                                     x.MethodInfo.GetParameters()
                                                                      .Any(p =>
                                                                           p.ParameterType.Name
                                                                            .Equals(
                                                                                _domainNameToTypeNameMapper(extendedMedaType.DomainModel),
                                                                                StringComparison
                                                                                    .CurrentCultureIgnoreCase)));
                    if (candidate != null)
                        return candidate;
                };                        
               
                
            }

            // GET calls that can be resolved by Accept which has a Non-Canonical Media Type
            // only if it is the first item in Accept Header
            if (context.Request.Method.Method == "GET" && context.Request.Headers.Accept.Count > 0)
            {
                var extendedMediaType = context.Request.Headers.Accept.First()
                    .ExtractFiveLevelsOfMediaType(FiveLevelsOfMediaTypeFormatter.DefaultNonCanonicalMediaTypePattern);
                
                if (extendedMediaType != null && !string.IsNullOrEmpty(extendedMediaType.DomainModel))
                {
                    var matches = actionDescriptors.Where(x => x.MethodInfo.ReturnType.Name ==
                        _domainNameToTypeNameMapper(extendedMediaType.DomainModel));

                    if (matches.Count() == 1)
                        return matches.First();

                    if (matches.Count() > 1)
                    {
                        var theMatchBasedOnVersion = matches.FirstOrDefault(x => Assembly.GetAssembly(x.ReturnType).GetName().Version.ToString() == 
                            _domainNameToTypeNameMapper(extendedMediaType.Version));
                        if (theMatchBasedOnVersion != null)
                            return theMatchBasedOnVersion;
                    }
                }
            }

            return base.ResolveAmbiguousActions(context, actionDescriptors);
        }
    }
}
