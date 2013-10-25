using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FiveLevelsOfMediaType
{
    public static class HttpConfigurationExtensions
    {
        public static void AddFiveLevelsOfMediaType(this HttpConfiguration configuration)
        {
            // formatters
            configuration.Formatters.DecorateFormatters();

            // adding action selector
            configuration.Services.Replace(typeof(IHttpActionSelector), new MediaTypeBasedActionSelector());

        }
    }
}
