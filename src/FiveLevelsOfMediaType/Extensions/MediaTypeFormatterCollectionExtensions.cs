using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace FiveLevelsOfMediaType
{
    public static class MediaTypeFormatterCollectionExtensions
    {
        public static void DecorateFormatters(this MediaTypeFormatterCollection formatterCollection)
        {
            var formatters = formatterCollection.ToArray();
            formatterCollection.Clear();
            foreach (var formatter in formatters)
            {
                formatterCollection.Add(new FiveLevelsOfMediaTypeFormatter(formatter));
            }
        }
    }
}
