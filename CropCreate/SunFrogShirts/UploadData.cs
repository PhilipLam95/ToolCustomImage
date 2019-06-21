using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunFrogShirts
{
    class UploadData
    {
        public int ArtOwnerID = 0;
        public Enums.Category Category = Enums.Category.Automotive;
        public string Collections = "";
        public string Description = "";
        public bool IAgree = true;
        public List<string> Keywords = new List<string>();
        public string Title = "";
        public string imageBack = "";
        public bool showStoreFront = false;
        public string imageFront = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" id=\"SvgjsSvg1000\" version=\"1.1\" width=\"2400\" height=\"3200\" viewBox=\"311.00000000008 150 387.99999999984004 517.33333333312\"><g id=\"SvgjsG1052\" transform=\"scale(0.15749999999993336 0.15749999999993336) translate(2006.349206350563 1151.7089947094153)\"><image id=\"SvgjsImage1053\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xlink:href=\"__dataURI:0__\" width=\"2400\" height=\"2886\"></image></g><defs id=\"SvgjsDefs1001\"></defs></svg>";
        public List<UploadImages> images = new List<UploadImages>();
        public List<UploadTypes> types = new List<UploadTypes>();
    }
}
