using HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunFrogShirts
{
    public class Accounts
    {
        public string EMAIL { get; set; }
        public string PASS { get; set; }

        public string SELLER_ID { get; set; }

        public Accounts() { }

        public Accounts(string email, string pass)
        {
            EMAIL = email;
            PASS = pass;
        }
    }
}
