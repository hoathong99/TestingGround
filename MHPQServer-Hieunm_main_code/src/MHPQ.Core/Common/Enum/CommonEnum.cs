using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Common.Enum
{
    public static partial class CommonENum
    {

        public enum ROLE_LEVEL
        {
            [EnumDisplayString("ADMIN")]
            SA = 0,
            [EnumDisplayString("Quản lý cư dân")]
            CITIZEN_MANAGER = 1,

        }
    }
}
