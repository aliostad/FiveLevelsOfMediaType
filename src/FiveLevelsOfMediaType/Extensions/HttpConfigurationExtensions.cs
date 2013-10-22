using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FiveLevelsOfMediaType
{
    public static class HttpConfigurationExtensions
    {
        public static void AddFiveLevelsOfMediaType(this HttpConfiguration configuration)
        {
            // formatters
            configuration.Formatters.DecorateFormatters();


        }
    }
}
