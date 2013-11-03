using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using ApiActionSelection.System.Web.Http.Controllers;

namespace FiveLevelsOfMediaType
{
    public class MediaTypeBasedActionSelector : ApiActionSelector
    {
        protected override ReflectedHttpActionDescriptor ResolveAmbiguousActions(HttpControllerContext context,
            IEnumerable<ReflectedHttpActionDescriptor> actionDescriptors)
        {
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
                                                                                extendedMedaType.DomainModel,
                                                                                StringComparison
                                                                                    .CurrentCultureIgnoreCase)));
                    if (candidate != null)
                        return candidate;
                };                        
               
                
            }

            return base.ResolveAmbiguousActions(context, actionDescriptors);
        }
    }
}
