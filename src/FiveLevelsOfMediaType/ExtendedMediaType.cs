using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveLevelsOfMediaType
{
    public class ExtendedMediaType
    {
        public bool? IsText { get; set; } 
        public string DomainModel { get; set; } 
        public string Version { get; set; } 
        public string Schema { get; set; } 
        public string Format { get; set; } 
    }
}
