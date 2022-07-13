using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializationEditor
{
    class EnumMember
    {
        public string name;
        public dynamic value;

        public EnumMember(string name, dynamic value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
