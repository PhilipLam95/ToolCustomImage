using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolCusTomImage.Common
{
    class SubPattern:Pattern
    {
        private string  pattern;

        public override string _Pattern
        {
            get
            {
                return pattern;
            }

            set

            {
                if (!string.IsNullOrEmpty(value))

                {
                    pattern = value;
                }

                else
                {
                    pattern = "No Value";
                }
            }
        }


        public override string GetPattern(int check)
        {
            if (check == 1)
            {
                pattern = "*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            }
            if (check == 2)
            {
                pattern = "*.txt";

            }
            if (check == 3 || check  == 0)
            {
                pattern = "*.*";
            }
            return pattern;
        }
    }
}
