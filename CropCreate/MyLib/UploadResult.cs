using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class UploadResult
    {
        public string description = "";
        public List<Product> products = new List<Product>();
        public string artFront = "";
        public double executionTime = 0;
        public string artBack = "";
        public int result = 0;
       
    }

    public class Product
    {
        public string pageName = "";
        public string imageFront = "";
        public string imageBack = "";
        public string color = "";
        public int id = 0;
        public int mockupGroupId = 0;
    }
}
