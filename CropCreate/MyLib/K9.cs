using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    class K9
    {

        public class Rootobject
        {
            public string description { get; set; }
            public Product[] products { get; set; }
            public string artFront { get; set; }
            public float executionTime { get; set; }
            public string artBack { get; set; }
            public int result { get; set; }
        }

        public class Product
        {
            public string pageName { get; set; }
            public string imageFront { get; set; }
            public string imageBack { get; set; }
            public int mockupId { get; set; }
            public string color { get; set; }
            public int id { get; set; }
            public int mockupGroupId { get; set; }
        }

    }
}
