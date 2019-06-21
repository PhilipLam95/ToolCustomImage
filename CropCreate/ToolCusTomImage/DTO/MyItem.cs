using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropCreate.DTO
{
    class MyItem
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public decimal price { get; set; }
        public decimal sale_price { get; set; }
        public decimal regular_price { get; set; }

        public DateTime date_modified { get; set; }

        public string sku { get; set; }

        public string type { get; set; }
        public bool manage_stock { get; set; }
        public bool in_stock { get; set; }




    }
}
