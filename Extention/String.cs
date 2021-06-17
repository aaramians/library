using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Extention
{
    public static partial class Extentions
    {
        public static bool IsIn(this string value, params string[] Items)
        {
            foreach (string s in Items)
                if (s == value)
                    return true;
            return false;
        }
    }
}
