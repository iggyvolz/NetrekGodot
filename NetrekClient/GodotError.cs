using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetrekGodot.NetrekClient
{
    class GodotError : Exception
    {
        public string Name { get; private set; }
        public GodotError(int errorCode)
        {
            Name = typeof(GodotError).GetEnumName(errorCode);
        }
        public override string ToString()
        {
            return Name;
        }
        public static int Assert(int errorCode)
        {
            if (errorCode != 0)
            {
                throw new GodotError(errorCode);
            }
            return errorCode;
        }
        public static T Assert<T>(object[] errorList)
        {
            Assert((int)errorList[0]);
            return (T)errorList[1];
        }
    }
}
