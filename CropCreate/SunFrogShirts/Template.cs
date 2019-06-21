using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunFrogShirts
{
    public class Template
    {
        public string Title = "{1}";
        public string Description = "{1}";
        public string Keywords = "{1}";
        public string Collection = "{1}";
        public bool showStoreFront = false;
        public bool UploadBack = false;
        public Enums.Category Category = Enums.Category.Automotive;
        public Enums.ShirtsType DefType = Enums.ShirtsType.Guys;
        public Enums.UploadType UploadType = Enums.UploadType.Shirts;
        public string DefColors = "Black";
        public List<UploadTypes> Types = new List<UploadTypes>();

        public bool Hidden1 = false;//Exclude from SunFrog site search results
        public bool Hidden2 = false;//Do not allow Google to index.
        public bool Hidden3 = false;//Isolate this design. This change cannot be reversed.
    }
}
