using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDose.WcfLib
{
    public static class Extensions
    {
        public static bool IsOk(this IServiceResult thisValue)
            => thisValue == null ? false : thisValue.StatusAsInt == 0;
    }
}
