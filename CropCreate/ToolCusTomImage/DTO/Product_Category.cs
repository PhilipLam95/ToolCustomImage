using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropCreate.DTO
{
    class Product_Category
    {

        public int? id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int? parent { get; set; }
        public string description { get; set; }
        public object display { get; set; }
        public string image { get; set; }
        public int? count { get; set; }
    }
}
