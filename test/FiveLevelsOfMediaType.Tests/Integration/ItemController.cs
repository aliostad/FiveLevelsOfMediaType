using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FiveLevelsOfMediaType.Tests.Integration
{
    public class ItemController : ApiController
    {
        public Item Get(int id)
        {
            return new Item();
        }
        public SuperItem GetSuper(int id)
        {
            return new SuperItem();
        }
    }


    public class Item
    {
        
    }

    public class SuperItem
    {
        
    }

}
