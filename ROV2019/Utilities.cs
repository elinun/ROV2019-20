using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019
{
    public class Utilities
    {
        public static TValue TryGet<Tkey, TValue>(Tkey key, Dictionary<Tkey, TValue> values)
        {
            values.TryGetValue(key, out TValue ret);
            return ret;
        }

    }
}
