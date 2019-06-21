using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Template
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; } 
        public bool showStoreFront { get; set; }
        public bool UploadBack { get; set; }
        public int Category { get; set; }
        public string DefType { get; set; }
        public string UploadType { get; set; }
        public string DefColors { get; set; }
        public string Server { get; set; }
        public string DirectoryGetImage { get; set; }
        public string username { get; set; }
        public string Domain { get; set; }
        public string wc_key { get; set; }
        public string ws_key { get; set; }
       
        public int Thread { get; set; }

        public double Sleep { get; set; }

        public List<UploadTypes> Types = new List<UploadTypes>();


        //public List<string> optionAttriBute = new List<string>();
    }
}
