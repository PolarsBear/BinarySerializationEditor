using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializationEditor
{
    public static class Utils
    {
        public static bool IsSimple(Type type)
        {
            return type.IsPrimitive
              || type.Equals(typeof(string));
        }
    }
}
